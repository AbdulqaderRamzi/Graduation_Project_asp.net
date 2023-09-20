using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Areas.Learner.Controllers
{
    [Area("Learner")]
    public class DjangoAuthenticationController : Controller
    {
        public IActionResult Change_Password()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Permissions_Authorization()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
