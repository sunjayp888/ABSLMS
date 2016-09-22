using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABS_LMS.Models;
using ABS_LMS.Service.Interface;
using Microsoft.AspNet.Identity.Owin;

namespace ABS_LMS.Controllers
{

    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEventService _eventService;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public HomeController(IEmployeeService employeeService, IEmployeeLeaveService employeeLeaveService, IEventService eventService)
        {
            _employeeService = employeeService;
            _eventService = eventService;
        }
        private ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                return _userManager;
            }
        }

        public ActionResult Index()
        {
            var today = DateTime.Today;
            var events = _eventService.GetEvents().Where(e => Convert.ToDateTime(e.DisplayStartDate) <= today && e.DisplayEndDate > today).ToList();
            var employees = _employeeService.GetEmployees().Where(s => Convert.ToDateTime(s.DOB).Day == today.Day
                            && Convert.ToDateTime(s.DOB).Month == today.Month && !s.LeavingDateUTC.HasValue
                            ).ToList();

            var model = new HomeViewModel
            {
                EmployeeBirthday = employees,
                Events = events
            };
            return View(model);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}