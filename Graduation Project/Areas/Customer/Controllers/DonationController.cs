using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Graduation_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DonationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DonationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Create(int? amount)
        {
            ViewBag.Amount = amount;
            return View();
        }
        public IActionResult Donate(int donate)
        {
            var domain = "https://localhost:44335/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Customer/Donation/DonateConfirmation?amount={donate}",
                CancelUrl = domain + $"Customer/Donation/Create?amount={donate}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            var AmountInCents = (long)(donate * 100);

            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = AmountInCents,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Donation"
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
        public IActionResult DonateConfirmation(int amount)
        {
            Donation donation = new Donation()
            {
                Amount = amount,
                DonatedAt = DateTimeOffset.Now,
            };
            _unitOfWork.Donation.Add(donation);
            _unitOfWork.Save();
            return View();
        }
    }
}
