using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Graduation_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class InquiryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InquiryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string type, string content)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            Inquiry inquiry = new Inquiry()
            {
                Type = type,
                Content = content,
                User = user,
                CreatedAt = DateTimeOffset.Now
            };
            _unitOfWork.Inquiry.Add(inquiry);
            _unitOfWork.Save();
            return RedirectToAction("index", "Home");
        }
    }
}
