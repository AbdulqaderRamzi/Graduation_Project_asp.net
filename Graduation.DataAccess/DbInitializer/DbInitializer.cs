using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Graduation.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _unitOfWork = unitOfWork;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                Community community = new Community();
                _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("Learner")).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("Supporter")).GetAwaiter().GetResult();
                _userManager.CreateAsync(new User
                {
                    UserName = "khalod.zeko@gmail.com",
                    Email = "khalod.zeko@gmail.com",
                    Name = "Admin",
                    Role = "Admin",
                    EmailConfirmed = true,
                }, "Admin123*").GetAwaiter().GetResult();
                User user = _db.Users.FirstOrDefault(u => u.Email == "khalod.zeko@gmail.com");
                community.Users.Add(user);
                ChatRoom chatRoom = new ChatRoom()
                {
                    User = user,
                };
                _unitOfWork.ChatRoom.Add(chatRoom);
                _unitOfWork.Community.Add(community);
                _unitOfWork.Save();
                _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
            }
            return;
        }
    }
}
