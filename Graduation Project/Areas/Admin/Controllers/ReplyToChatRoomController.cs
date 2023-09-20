using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Graduation_Project.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Supporter")]
    [Area("Admin")]
    public class ReplyToChatRoomController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ReplyToChatRoomController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<ChatRoom> chatRooms = _db.ChatRooms.Include(c => c.User).Include(c => c.ChatRoomMessages).Where(c => c.ChatRoomMessages.Any(cr => !cr.Closed)).ToList();
            return View(chatRooms);
        }
        public IActionResult Details(int id)
        {
            ChatRoom chatRoom = _db.ChatRooms.Include(c => c.User).Include(c => c.ChatRoomMessages).ThenInclude(i=> i.Images).FirstOrDefault();
            return View(chatRoom);
        }
        public IActionResult Reply(int id, string content, List<IFormFile>? images, IFormFile? video)
        {
            if (id == 0)
            {
                return NotFound();
            }
            if (content != null || images.Count() > 0 || video != null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = _unitOfWork.User.Get(u => u.Id == userId);
                ChatRoom chatRoom = _unitOfWork.ChatRoom.Get(i => i.Id == id, includeProperties: "ChatRoomMessages.Images");
                ChatRoomMessage chatRoomMessage = new ChatRoomMessage()
                {
                    Content = content,
                    User = user,
                    ChatRoom = chatRoom,
                    SendAt = DateTimeOffset.Now,
                    Closed = true
                };
                _unitOfWork.ChatRoomMessage.Add(chatRoomMessage);
                List<ChatRoomMessage> readMessage = _unitOfWork.ChatRoomMessage.GetAll(i=>i.ChatRoom==chatRoom).ToList();
                foreach(ChatRoomMessage message in readMessage)
                {
                    message.Closed = true;
                    _unitOfWork.Save();
                }
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
                return RedirectToAction("Index");
            }
            return RedirectToAction("Details", new { id = id });
        }
    }
}
