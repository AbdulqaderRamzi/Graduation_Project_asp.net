using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;

namespace Graduation_Project.Areas.Learner.Controllers
{
    [Area("Learner")]
    public class DjangoQueryController : Controller
    {
        public IActionResult Field_lookups()
        {
            return View();
        }
        public IActionResult Methods()
        {
            return View();
        }
        public IActionResult Operators()
        {
            return View();
        }
        public IActionResult HowToQuery()
        {
            return View();
        }
    }
}
