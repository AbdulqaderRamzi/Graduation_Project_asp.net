using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Graduation_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ContactUsController : Controller
    {
        [HttpGet]
        public IActionResult Contactus()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contactus(string from, string fullname, string phonenumber, string content, string subject)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string smtpServer = configuration["SmtpSettings:SmtpServer"];
            int smtpPort = int.Parse(configuration["SmtpSettings:SmtpPort"]);
            string smtpUsername = configuration["SmtpSettings:SmtpUsername"];
            string smtpPassword = configuration["SmtpSettings:SmtpPassword"];
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fullname, from));
            message.To.Add(new MailboxAddress("Khaled Alshammi", "kkhhaa2002yl@gmail.com"));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = $"{content} \n\n{phonenumber}." };

            using (var client = new SmtpClient())
            {
                client.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(smtpUsername, smtpPassword);
                client.Send(message);
                client.Disconnect(true);
            }
            return View();
        }
    }
}
