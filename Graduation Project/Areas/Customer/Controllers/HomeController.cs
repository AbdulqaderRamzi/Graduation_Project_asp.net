using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using Graduation_Project.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Diagnostics;
using System.Security.Claims;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Graduation_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ApplyTojob()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult ApplyTojob(IFormFile CV)
        {
            string fileExtension = Path.GetExtension(CV.FileName).ToLower();
            if (fileExtension == ".pdf" || fileExtension == ".xls" || fileExtension == ".xlsx")
            {
                var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                string smtpServer = configuration["SmtpSettings:SmtpServer"];
                int smtpPort = int.Parse(configuration["SmtpSettings:SmtpPort"]);
                string smtpUsername = configuration["SmtpSettings:SmtpUsername"];
                string smtpPassword = configuration["SmtpSettings:SmtpPassword"];

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = _unitOfWork.User.Get(u => u.Id == userId);

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = "";
                if (CV != null)
                {
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(CV.FileName);
                    string newsPath = Path.Combine(wwwRootPath, @"files/cvs");
                    if (!Directory.Exists(newsPath))
                    {
                        Directory.CreateDirectory(newsPath);
                    }
                    using (var fileStream = new FileStream(Path.Combine(newsPath, fileName), FileMode.Create))
                    {
                        CV.CopyTo(fileStream);
                    }
                }
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(null, user.Email));
                message.To.Add(new MailboxAddress("Khaled Alshammi", "kkhhaa2002yl@gmail.com"));
                message.Subject = $"Job";
/*                message.Body = new TextPart("plain") { Text = "You have applied successfully" };*/
                var attachment = new MimePart("application", "octet-stream")
                {
                    Content = new MimeContent(CV.OpenReadStream()),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = fileName
                };
/*                var plainTextPart = new TextPart("plain")
                {
                    Text = "You have applied successfully"
                };*/
                var multipart = new Multipart("mixed");
       /*         multipart.Add(plainTextPart);*/
                multipart.Add(attachment);
                message.Body = multipart;
                Applicant applicant = new Applicant()
                {
                    User = user,
                    CV = Path.Combine("files/cvs", fileName),
                    AppliedAt = DateTimeOffset.Now
                };
                _unitOfWork.Applicant.Add(applicant);
                _unitOfWork.Save();
                user.Applicants.Add(applicant);
                _unitOfWork.Save();

                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    client.Authenticate(smtpUsername, smtpPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.CVError = "Only PDF and Excel files are allowed.";
                return RedirectToAction("ApplyTojob");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
