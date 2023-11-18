using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
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
        public IActionResult Index(string? user)
        {
            if (user == null)
            {
                List<ChatRoom> chatRooms = _db.ChatRooms.Include(c => c.User).Include(c => c.ChatRoomMessages).ThenInclude(c=>c.User).Where(i=>i.ChatRoomMessages.Any(u=>u.User.Role!="Supporter" && u.User.Role != "Admin")).ToList();
                return View(chatRooms);
            }
            else
            {
                List<ChatRoom> chatRooms = _db.ChatRooms.Include(c => c.User).Include(c => c.ChatRoomMessages).ThenInclude(c => c.User).Where(i => i.User.Name.Contains(user) || i.User.Email.Contains(user)).Where(i => i.ChatRoomMessages.Any(u => u.User.Role != "Supporter" && u.User.Role != "Admin")).ToList();
                if (chatRooms.Count == 0)
                {
                    List<ChatRoom> chats = _db.ChatRooms.Include(c => c.User).Include(c => c.ChatRoomMessages).ThenInclude(c => c.User).Where(i => i.ChatRoomMessages.Any(u => u.User.Role != "Supporter" && u.User.Role != "Admin")).ToList();
                    return View(chats);
                }
                return View(chatRooms);
            }

        }
        public IActionResult Details(int id)
        {
            ChatRoom chatRoom = _db.ChatRooms
                .Include(c => c.User)
                .Include(c => c.ChatRoomMessages)
                .ThenInclude(i => i.Images)
                .Include(c => c.ChatRoomMessages)
                .ThenInclude(c=> c.User)
                .FirstOrDefault(i=> i.Id == id);
            return View(chatRoom);
        }
        public IActionResult Reply(int id, string content, List<IFormFile>? images)
        {
            if (id == 0)
            {
                return NotFound();
            }
            if (content != null || images.Count() > 0)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = _unitOfWork.User.Get(u => u.Id == userId);
                ChatRoom chatRoom = _unitOfWork.ChatRoom.Get(i => i.Id == id, includeProperties: "ChatRoomMessages.Images");
                List<ChatRoomMessage> readMessage = _unitOfWork.ChatRoomMessage.GetAll(i=>i.ChatRoom==chatRoom, includeProperties:"User").ToList();
                foreach(ChatRoomMessage message in readMessage)
                {
                    if (message.User.Role != "Supporter" && message.User.Role != "Admin")
                    {
                        message.Closed = true;
                        _unitOfWork.Save();
                    }
                }
                ChatRoomMessage chatRoomMessage = new ChatRoomMessage()
                {
                    Content = content,
                    User = user,
                    ChatRoom = chatRoom,
                    SendAt = DateTimeOffset.Now,
                    Closed = false
                };
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
                return RedirectToAction("Details", new { id = id });
            }
            return RedirectToAction("Index");
        }
    }
}
