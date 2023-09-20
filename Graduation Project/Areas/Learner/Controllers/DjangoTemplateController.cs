using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Areas.Learner.Controllers
{
    [Area("Learner")]
    public class DjangoTemplateController : Controller
    {
        public IActionResult Custom_Filter()
        {
            return View();
        }
        public IActionResult Custom_Tag()
        {
            return View();
        }
        public IActionResult Extend()
        {
            return View();
        }
        public IActionResult Filter()
        {
            return View();
        }
        public IActionResult Include()
        {
            return View();
        }
        public IActionResult Tag()
        {
            return View();
        }
        public IActionResult Template_Configuration()
        {
            return View();
        }
    }
}
