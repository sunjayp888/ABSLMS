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
        private readonly IEmployeeLeaveService _employeeLeaveService;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public HomeController(IEmployeeService employeeService, IEmployeeLeaveService employeeLeaveService)
        {
            _employeeService = employeeService;
            _employeeLeaveService = employeeLeaveService;
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
            DateTime today = DateTime.Today;
            var employees = _employeeService.GetEmployees().Where(s=>Convert.ToDateTime(s.DOB).Day== today.Day && Convert.ToDateTime(s.DOB).Month==today.Month);
            var model = new HomeViewModel
            {
                EmployeeBirthday = employees.ToList()
              
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