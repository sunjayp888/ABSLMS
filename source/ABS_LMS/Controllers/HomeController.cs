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
        private DateTime Today => DateTime.Today;

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

            var events = _eventService.GetEvents().Where(e => Convert.ToDateTime(e.DisplayStartDate) <= Today && e.DisplayEndDate >= Today).ToList();

            var announcement = GetAnnouncementForBirthDay();
            announcement.AddRange(GetAnnouncementForYearComplete());
             
            var model = new HomeViewModel
            {
                //EmployeeBirthday = employees,
                Announcements = announcement,
                Events = events
            };
            return View(model);

        }

        private List<Announcement> GetAnnouncementForBirthDay()
        {
            var data = _employeeService.GetEmployees().Where(s => Convert.ToDateTime(s.DOB).Day == Today.Day
                            && Convert.ToDateTime(s.DOB).Month == Today.Month && !s.LeavingDateUTC.HasValue
                             ).ToList();

            return data.Select(item => new Announcement()
            {
                Type = "B", Content = string.Format("Happy Birthday"), Designation = item.Designation,
                Name = string.Format("{0} {1} - {2}",item.FirstName,item.LastName,item.Designation),
                EmployeeImage = item.EmployeeImage,
            }).ToList();
        }

        private List<Announcement> GetAnnouncementForYearComplete()
        {
            var y = Today.Year;
            var d = Today.Day;
            var m = Today.Month;
            var data = _employeeService.GetEmployees().Where(s => Convert.ToDateTime(s.DOJ).Day == Today.Day && Convert.ToDateTime(s.DOJ).Month == Today.Month && Convert.ToDateTime(s.DOJ).Year < Today.Year && !s.LeavingDateUTC.HasValue && s.EmployeeCode!="1002").ToList();
            return data.Select(item => new Announcement()
            {
                Type = "Y",
                Content = string.Format("Congratulations on completion of {0} year of service at ABS", Today.Year- Convert.ToDateTime(item.DOJ).Year),
                Designation = item.Designation,
                Name = string.Format("On behalf of ABS family,We congratulate {0} - {1}  who has completed {2} of dedicated service at ABS", item.FirstName + ' ' + item.LastName,item.Designation, Today.Year - Convert.ToDateTime(item.DOJ).Year),
                EmployeeImage = item.EmployeeImage,
            }).ToList();
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