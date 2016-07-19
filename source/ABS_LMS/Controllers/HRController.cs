using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABS_LMS.Service.Interface;

namespace ABS_LMS.Controllers
{
    public class HRController : Controller
    {
        private readonly IEmployeeLeaveService _employeeLeaveService;
        private readonly IEmployeeService _employeeService;

        public HRController(IEmployeeLeaveService employeeLeaveService, IEmployeeService employeeService)
        {
            _employeeLeaveService = employeeLeaveService;
            _employeeService = employeeService;
        }

        //GET: HR
        public ActionResult Index()
        {
            //_employeeLeaveService. ().Where(e=>e.)
            return View();
        }
    }
}