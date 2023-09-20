using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Areas.Learner.Controllers
{
    [Area("Learner")]
    public class DjangoThirdPartyController : Controller
    {
        public IActionResult Jazzmin()
        {
            return View();
        }
        public IActionResult Phonenumbers()
        {
            return View();
        }
        public IActionResult RichText()
        {
            return View();
        }
    }
}
