using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Areas.Learner.Controllers
{
    [Area("Learner")]
    public class DjangoViewController : Controller
    {
        public IActionResult FBV()
        {
            return View();
        }
        public IActionResult Response_Types()
        {
            return View();
        }
        public IActionResult CBV_Create()
        {
            return View();
        }
        public IActionResult CBV_Update()
        {
            return View();
        }
        public IActionResult CBV_Delete()
        {
            return View();
        }
        public IActionResult CBV_Detail()
        {
            return View();
        }
        public IActionResult CBV_Form()
        {
            return View();
        }
        public IActionResult CBV_List()
        {
            return View();
        }
        public IActionResult CBV_Template()
        {
            return View();
        }
    }
}
