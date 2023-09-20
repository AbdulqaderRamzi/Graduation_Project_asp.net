using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Graduation_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Supporter")]
    public class ReplyToInquiryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReplyToInquiryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(bool unreplied)
        {
            List<Inquiry> inquiries = null;
            if (unreplied == true)
            {
                inquiries = _unitOfWork.Inquiry.GetAll(i=>i.Reply == false, includeProperties: "User").ToList();
            }
            else
            {
                inquiries = _unitOfWork.Inquiry.GetAll(includeProperties: "User").ToList();
            }
            return View(inquiries);
        }
        [HttpGet]
        public IActionResult Reply(int id)
        {
            Inquiry inquiry = _unitOfWork.Inquiry.Get(u => u.Id == id);
            return View(inquiry);
        }
        [HttpPost]
        public IActionResult Reply(int id, string reply)
        {
            if (id > 0 && reply != null)
            {
                Inquiry inquiry = _unitOfWork.Inquiry.Get(u => u.Id == id, includeProperties:"User");
                inquiry.RepliedAt = DateTimeOffset.Now;
                inquiry.Reply = true;
                _unitOfWork.Save();
                var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                string smtpServer = configuration["SmtpSettings:SmtpServer"];
                int smtpPort = int.Parse(configuration["SmtpSettings:SmtpPort"]);
                string smtpUsername = configuration["SmtpSettings:SmtpUsername"];
                string smtpPassword = configuration["SmtpSettings:SmtpPassword"];
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Khaled Alshammi", "kkhhaa2002yl@gmail.com"));
                message.To.Add(new MailboxAddress(null, inquiry.User.Email));
                message.Subject = $"Reply to {inquiry.Type} inquiry";
                message.Body = new TextPart("plain") { Text = reply };
                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    client.Authenticate(smtpUsername, smtpPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("reply", new { id = id });
        }
    }
}
