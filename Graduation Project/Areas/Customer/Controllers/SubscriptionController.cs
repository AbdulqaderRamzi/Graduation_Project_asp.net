using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Stripe.Checkout;
using System.Security.Claims;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Graduation_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class SubscriptionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubscriptionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Subscriptions()
        {
            List<SubscriptionType> subscriptionTypes = _unitOfWork.SubscriptionType.GetAll().ToList();
            return View(subscriptionTypes);
        }
        [Authorize]
        public IActionResult Subscription(int id)
        {
            SubscriptionType subscriptionType = _unitOfWork.SubscriptionType.Get(i => i.Id == id);
            var domain = "https://localhost:44335/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Customer/Subscription/SubscriptionConfirmation?id={id}",
                CancelUrl = domain + $"Customer/Subscription/Subscriptions",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            var PriceInCents = (long)(subscriptionType.Price * 100);
            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = PriceInCents,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = subscriptionType.Title
                    }
                },
                Quantity = 1
            };
            options.LineItems.Add(sessionLineItem);
            var service = new SessionService();
            Session session = service.Create(options);
            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }
        [Authorize]
        public IActionResult SubscriptionConfirmation(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            SubscriptionType subscriptionType = _unitOfWork.SubscriptionType.Get(i => i.Id == id);
            Subscription subscription = new Subscription()
            {
                SubscriptionType = subscriptionType,
                User = user,
            };
            _unitOfWork.Subscription.Add(subscription);
            _unitOfWork.Save();
            subscriptionType.Subscriptions.Add(subscription);
            user.Subscriptions.Add(subscription);
            user.Explanation += subscriptionType.Explanation;
            user.Solution += subscriptionType.Solution;

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string smtpServer = configuration["SmtpSettings:SmtpServer"];
            int smtpPort = int.Parse(configuration["SmtpSettings:SmtpPort"]);
            string smtpUsername = configuration["SmtpSettings:SmtpUsername"];
            string smtpPassword = configuration["SmtpSettings:SmtpPassword"];
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Khaled Alshammi", "kkhhaa2002yl@gmail.com"));
            message.To.Add(new MailboxAddress(null, user.Email));
            message.Subject = $"{subscriptionType.Title} Subscription";
            message.Body = new TextPart("plain") { Text = "You have successfully subscribed." };
            using (var client = new SmtpClient())
            {
                client.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(smtpUsername, smtpPassword);
                client.Send(message);
                client.Disconnect(true);
            }
            _unitOfWork.Save();
            return View();
        }
    }
}
