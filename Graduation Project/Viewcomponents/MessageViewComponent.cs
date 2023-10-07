using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Graduation_Project.Viewcomponents
{
    public class MessageViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public MessageViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            ChatRoom chatRoom = _unitOfWork.ChatRoom.Get(i=>i.User == user, includeProperties: "ChatRoomMessages");
            int count = chatRoom.ChatRoomMessages.Count(i => i.Closed == false);
            return View(count);
        }
    }
}
