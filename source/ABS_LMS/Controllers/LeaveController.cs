using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Xsl;
using ABS_LMS.Helper;
using ABS_LMS.Models;
using ABS_LMS.Service.Interface;
using ABS_LMS.Service.Model;
using ABS_LMS.Models.Security;

namespace ABS_LMS.Controllers
{
    public class LeaveController : Controller
    {
        private readonly IEmployeeLeaveService _employeeLeaveService;
        private readonly IEmployeeService _employeeService;
        private readonly IHolidayService _holidayService;

        public LeaveController(IEmployeeLeaveService employeeLeaveService, IEmployeeService employeeService, IHolidayService holidayService)
        {
            _employeeLeaveService = employeeLeaveService;
            _employeeService = employeeService;
            _holidayService = holidayService;
        }

        // GET: Leave
        public ActionResult Index(int id)
        {
            return Authorization.HasAccess(Convert.ToString(id), () =>
            {
                var leaveDetails = _employeeLeaveService.GetEmployeeLeaveDetails(id);

            var model = leaveDetails.Select(employeeLeave => new EmployeeLeaveViewModel
            {

                EmployeeLeaveDetails = employeeLeave,


            }).ToList();

            return View(model);
            });
        }
        // GET: Leave

        public ActionResult LeavePendingForApproval(int id)
        {

            return Authorization.HasAccess(Convert.ToString(id), () =>
            {
                var leaveDetails = _employeeLeaveService.GetLeaveDetailsByApprovedId(id);

            var model = leaveDetails.Select(employeeLeave => new EmployeeLeaveViewModel
            {

                EmployeeLeaveDetails = employeeLeave,

            }).ToList();
            return View(model);
            });
        }


        [Authorize(Roles = "HR")]
        public ActionResult ApprovedLeaveDetails(int id)
        {

            return Authorization.HasAccess(Convert.ToString(id), () =>
            {
                var leaveDetails = _employeeLeaveService.GetApprovedLeaves();

                var model = leaveDetails.Select(employeeLeave => new EmployeeLeaveViewModel
                {

                    EmployeeLeaveDetails = employeeLeave,

                }).ToList();
                return View(model);
            });
        }
        public ActionResult Create(int id)
        {


            return Authorization.HasAccess( Convert.ToString(id), () =>
            {
                var leavetype = _employeeLeaveService.GetLeaves().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.LeaveTypeId.ToString()

                });

                var enumValues = Enum.GetValues(typeof(LeaveStatus)) as LeaveStatus[];
                if (enumValues == null)
                    return null;

                var leaveStatus = enumValues.Select(enumValue => new SelectListItem
                {

                    Value = ((int)enumValue).ToString(),
                    Text = _employeeLeaveService.GetEnumsNameById(Convert.ToInt32(enumValue))
                }).ToList();
                var manager = from e in _employeeService.GetEmployees()
                              join m in _employeeService.GetEmployees() on e.ReportingManager equals m.EmployeeId
                              where e.EmployeeId.Equals(id)
                              select new
                              {
                                  managerId = e.ReportingManager,
                                  managerName = m.FirstName + " " + m.LastName,
                              };
                var model = new EmployeeLeaveViewModel
                {
                    EmployeeLeaveDetails = new EmployeeLeave { EmployeeId = id },
                    LeaveType = leavetype,
                    LeaveStatusEnums = leaveStatus,
                    LeaveSummaries = _employeeLeaveService.GetLeaveSummary(id)
                };
                model.EmployeeLeaveDetails.ApprovedPersonName = manager.FirstOrDefault().managerName ?? "";
                model.EmployeeLeaveDetails.ApprovedBy = manager.FirstOrDefault().managerId;
                return View(model);
            });


        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(int id, EmployeeLeaveViewModel employeeleave)
        {
            try
            {
                var newleave = employeeleave.EmployeeLeaveDetails;
                _employeeLeaveService.AddEmployeeLeaveDetails(newleave);
                return RedirectToAction("Index/" + id + "");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id ,int leaveId)
        {
            return Authorization.HasAccess(Convert.ToString(id), () =>
            {
                var leavedetails = _employeeLeaveService.GetLeavedetails(leaveId);
                var leavetype = _employeeLeaveService.GetLeaves().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.LeaveTypeId.ToString()
                });

            var enumValues = Enum.GetValues(typeof(LeaveStatus)) as LeaveStatus[];
            if (enumValues == null)
                return null;

            var leaveStatus = enumValues.Select(enumValue => new SelectListItem
            {
                Value = ((int)enumValue).ToString(),
                Text = _employeeLeaveService.GetEnumsNameById(Convert.ToInt32(enumValue))
            }).ToList().Take(2);

            var model = new EmployeeLeaveViewModel
            {
                EmployeeLeaveDetails = leavedetails,
                LeaveType = leavetype,
                LeaveStatusEnums = leaveStatus,
            };

            return View(model);
            });
        }
        [HttpPost]
        public ActionResult Edit(int leaveId, EmployeeLeaveViewModel employeeleave)
        {
            try
            {
                var leaveDetails = employeeleave.EmployeeLeaveDetails;
                _employeeLeaveService.UpdateEmployeeLeaveDetails(leaveId, leaveDetails);
                return RedirectToAction("Index", new { id = employeeleave.EmployeeLeaveDetails.EmployeeId });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult LeaveStatus(string status, int historyid)
        {
            int result = _employeeLeaveService.UpdateLeaveStatus(status, historyid);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        
        public ActionResult EmployeeReportCsvDownload(int id)
        {
            var leaveDetails = _employeeLeaveService.GetEmployeeLeaveDetails(id);
             
            return File(leaveDetails.ToDataTable().ToCsvStream(), "text/csv",
                string.Format("Employees-{0:yyyy-MM-dd-hh-mm-ss}.csv", DateTime.Now));
        }
        //public string GetEnumsNameById(int enumId)
        //{
        //    LeaveStatus enumDisplayStatus = (LeaveStatus)enumId;
        //    string stringValue = enumDisplayStatus.ToString();
        //    return stringValue;
        //}

        
        //[HttpPost]
        public ActionResult GetActualLeaveDaysCount(DateTime? startDate, DateTime? endDate)
            {
            var result = _holidayService.GetActualLeaveDaysCount(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
            return Json(new { Value = result},JsonRequestBehavior.AllowGet);
        }
    }
}