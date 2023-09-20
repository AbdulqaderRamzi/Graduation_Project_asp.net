using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using Graduation.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Graduation_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CommunityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CommunityController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            Community community = _unitOfWork.Community.Get(i => i.Id == 1, includeProperties: "Users,Messages");
            List<Message> messages = _unitOfWork.Message.GetAll(i=> i.Community.Id == community.Id, includeProperties: "User").ToList();
            CommunityVM communityVM = new CommunityVM()
            {
                Community = community,
                User = user
            };
            return View(communityVM);
        }

        public IActionResult CommunityMessage(int? replyTo, string? content, IFormFile? image)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            Community community = _unitOfWork.Community.Get(i => i.Id == 1, includeProperties: "Users,Messages.User");
            Message message = new Message()
            {
                Content = content,
                SendAt = DateTimeOffset.Now,
                User = user,
                Community = community,
            };
            if (replyTo != 0)
            {
                Message replyMessage = _unitOfWork.Message.Get(i => i.Id == replyTo, includeProperties:"User");
                message.ReplyTo = replyMessage;
            }
            _unitOfWork.Message.Add(message);
            _unitOfWork.Save();
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            if (image != null)
            {

                string fileExtension = Path.GetExtension(image.FileName);
                if (imageExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string messagePath = Path.Combine(wwwRootPath, @"images/Community/message/" + community.Id);
                    if (!Directory.Exists(messagePath))
                    {
                        Directory.CreateDirectory(messagePath);
                    }
                    if (!string.IsNullOrEmpty(message.Image))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, message.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(messagePath, fileName), FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                    message.Image = @"images/Community/message/" + community.Id + '/' + fileName;
                    _unitOfWork.Message.Update(message);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteCommunityMessage(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            Community community = _unitOfWork.Community.Get(i => i.Id == 1);
            Message message = _unitOfWork.Message.Get(i=>i.Id == id && i.User == user);
            if (message != null)
            {
                var ImagePath = Path.Combine(_webHostEnvironment.WebRootPath, message.Image.TrimStart('\\'));
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
                _unitOfWork.Message.Remove(message);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
