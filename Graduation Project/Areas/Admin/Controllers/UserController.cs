using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using Graduation.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string email){
            if (email != null)
            {
                List<User> users = _unitOfWork.User.GetAll(i=> i.Email.Contains(email)).ToList();
                return View(users);
            }
            else
            {
                List<User> users = _unitOfWork.User.GetAll().ToList();
                return View(users);
            }
        }
        public IActionResult UserRole(string id, bool admin, bool learner, bool supporter)
        {
            User user = _unitOfWork.User.Get(u => u.Id == id);
            var currentRoles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
            _userManager.RemoveFromRolesAsync(user, currentRoles).GetAwaiter().GetResult();

            if (admin)
            {
                user.Role = "Admin";
                _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
            }
            if (learner)
            {
                user.Role = "Learner";
                _userManager.AddToRoleAsync(user, "Learner").GetAwaiter().GetResult();
            }
            if (supporter)
            {
                user.Role = "Supporter";
                _userManager.AddToRoleAsync(user, "Supporter").GetAwaiter().GetResult();
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
