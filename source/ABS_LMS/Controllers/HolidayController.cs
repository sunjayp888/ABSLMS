using System.Web.Mvc;
using ABS_LMS.Models;
using ABS_LMS.Service.Interface;
using ABS_LMS.Service.Model;
using PagedList;

namespace ABS_LMS.Controllers
{
    [Authorize(Roles = ("Hr,Admin"))]
    public class HolidayController : Controller
    {
        private readonly IHolidayService _holidayService;

        public HolidayController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        public ActionResult Index(int pagenumber = 1, int pagesize = 10)
        {
            var publicHolidays = _holidayService.GetAllPublicHolidaysList();
            var model = new HolidayViewModel
            {
                Holidays = publicHolidays.ToPagedList(pagenumber, pagesize)
            };
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Holiday model)
        {
            if (!ModelState.IsValid) return View(model);
            if (model.HolidayId == 0)
                _holidayService.AddHoliday(model);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            _holidayService.DeleteHoliday(id);
            return Json(true, JsonRequestBehavior.DenyGet);
        }
    }
}