using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Areas.Learner.Controllers
{
    [Area("Learner")]
    public class DjangoFormController : Controller
    {
        public IActionResult Forms()
        {
            return View();
        }
        public IActionResult Html_Form()
        {
            return View();
        }
    }
}
