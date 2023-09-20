using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SubscriptionTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubscriptionTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<SubscriptionType>  subscriptionTypes = _unitOfWork.SubscriptionType.GetAll().ToList();
            return View(subscriptionTypes);
        }
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                SubscriptionType subscriptionType = _unitOfWork.SubscriptionType.Get(i => i.Id == id);
                _unitOfWork.SubscriptionType.Remove(subscriptionType);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id != 0 || id != null)
            {
                SubscriptionType subscriptionType = _unitOfWork.SubscriptionType.Get(e => e.Id == id);
                return View(subscriptionType);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(int id, SubscriptionType obj, IFormFile? file)
        {
            if (id != 0)
            {
                SubscriptionType subscriptionType = _unitOfWork.SubscriptionType.Get(i => i.Id == id);
                subscriptionType.Price = obj.Price;
                subscriptionType.Title = obj.Title;
                subscriptionType.Description = obj.Description;
                subscriptionType.Explanation = obj.Explanation;
                subscriptionType.Solution = obj.Solution;
                _unitOfWork.SubscriptionType.Update(subscriptionType);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(SubscriptionType subscriptionType, IFormFile? file)
        {
            if (subscriptionType != null)
            {
                _unitOfWork.SubscriptionType.Add(subscriptionType);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
