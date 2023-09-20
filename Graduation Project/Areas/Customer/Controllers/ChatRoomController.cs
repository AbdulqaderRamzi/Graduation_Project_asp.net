using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Graduation_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ChatRoomController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ChatRoomController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult UserChatRoom()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            /*ChatRoom chatRoom = _unitOfWork.ChatRoom.Get(i => i.User == user, includeProperties: "User, ChatRoomMessage.Image");*/
            ChatRoom chatRoom = _db.ChatRooms.Include(c => c.User).Include(c => c.ChatRoomMessages).ThenInclude(cm => cm.Images).FirstOrDefault(i => i.User == user);
            return View(chatRoom);
        }
        [HttpPost]
        public IActionResult Ask(string type, string content, List<IFormFile>? images, IFormFile? video)
        {
            if (type == "Explanation" || type == "Solution" || type == "Both")
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = _unitOfWork.User.Get(u => u.Id == userId);
                ChatRoom chatRoom = _db.ChatRooms.Include(c => c.User).Include(c => c.ChatRoomMessages).ThenInclude(cm => cm.Images).FirstOrDefault(i => i.User == user);
                ChatRoomMessage chatRoomMessage = new ChatRoomMessage()
                {
                    Content = content,
                    User = user,
                    ChatRoom = chatRoom,
                    SendAt = DateTimeOffset.Now,
                };
                if (user.Explanation > 0 && type == "Explanation")
                {
                    user.Explanation -= 1;
                    chatRoomMessage.Explanation = true;
                }
                if (user.Solution > 0 && type == "Solution")
                {
                    user.Solution -= 1;
                    chatRoomMessage.Solution = true;
                }
                if (user.Solution > 0 && user.Explanation > 0 && type == "Both")
                {
                    user.Explanation -= 1;
                    user.Solution -= 1;
                    chatRoomMessage.Explanation = true;
                    chatRoomMessage.Solution = true;
                }
                _unitOfWork.ChatRoomMessage.Add(chatRoomMessage);
                _unitOfWork.Save();
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (images != null)
                {
                    foreach (IFormFile file in images)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string messagePath = @"images\ChatRoomMessage\message-" + chatRoomMessage.Id;
                        string finalPath = Path.Combine(wwwRootPath, messagePath);
                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);
                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        Image image = new()
                        {
                            ChatRoomMessage = chatRoomMessage,
                            ImageUrl = @"\" + messagePath + @"\" + fileName
                        };
                        chatRoomMessage.Images.Add(image);
                        _unitOfWork.Save();
                    }
                }
                if (video != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(video.FileName);
                    string videoPath = Path.Combine(wwwRootPath, @"images/ChatRoomMessage/videos");
                    if (!Directory.Exists(videoPath))
                    {
                        Directory.CreateDirectory(videoPath);
                    }
                    if (!string.IsNullOrEmpty(chatRoomMessage.Video))
                    {
                        var oldVideoPath = Path.Combine(wwwRootPath, chatRoomMessage.Video.TrimStart('\\'));
                        if (System.IO.File.Exists(oldVideoPath))
                        {
                            System.IO.File.Delete(oldVideoPath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(videoPath, fileName), FileMode.Create))
                    {
                        video.CopyTo(fileStream);
                    }
                    chatRoomMessage.Video = @"images/ChatRoomMessage/videos/" + fileName;
                    _unitOfWork.Save();
                }
                return RedirectToAction("UserChatRoom");
            }
            return RedirectToAction("UserChatRoom");
        }
    }
}
