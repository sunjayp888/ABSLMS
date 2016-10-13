using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABS_LMS.Helper;
using ABS_LMS.Models;
using ABS_LMS.Service.Interface;
using ABS_LMS.Service.Model;
using ABS_LMS.Models.Security;
using ABS_LMS.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
namespace ABS_LMS.Controllers
{
    public class LeaveController : Controller
    {
        private readonly IEmployeeLeaveService _employeeLeaveService;
        private readonly IEmployeeService _employeeService;
        private readonly IHolidayService _holidayService;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        private ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                return _userManager;
            }
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                if (_roleManager == null)
                    _roleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();

                return _roleManager;
            }
        }

        public LeaveController(IEmployeeLeaveService employeeLeaveService, IEmployeeService employeeService, IHolidayService holidayService)
        {
            _employeeLeaveService = employeeLeaveService;
            _employeeService = employeeService;
            _holidayService = holidayService;
        }

        // GET: Leave

        public ActionResult Index(int id, int pagenumber = 1, int pagesize = 10)
        {
            return Authorization.HasAccess(Convert.ToString(id), () =>
            {
                var leaveDetails = _employeeLeaveService.GetEmployeeLeaveDetails(id).Where(s => s.LeaveStatus == (int)Service.Model.LeaveStatus.Apply);

                var model = new EmployeeLeaveIndexViewModel
                {
                    EmployeeLeaveDetails = leaveDetails.ToPagedList(pagenumber, pagesize),
                    FromDate = null,
                    ToDate = null,
                };
                return View(model);
            });
        }

        public ActionResult History(DateTime? FromDate, DateTime? ToDate, int pagenumber = 1, int pagesize = 10)
        {
            if (FromDate!=null && ToDate != null)
            {
                //DateTime is null
      
            return Authorization.HasAccess(Convert.ToString(HttpCurrentUser.EmployeeId), () =>
            {
                var leaveDetails =
                    _employeeLeaveService.GetEmployeeLeaveDetails(Convert.ToInt32(HttpCurrentUser.EmployeeId))
                                    .Where(e => (e.LeaveStartDate >= FromDate && e.LeaveStartDate <= ToDate)
                                   || (e.LeaveEndDate >= FromDate && e.LeaveEndDate <= ToDate)
                                   || (e.LeaveStartDate <= FromDate && e.LeaveEndDate >= ToDate));

                var model = new EmployeeLeaveIndexViewModel
                {
                    EmployeeLeaveDetails = leaveDetails.ToPagedList(pagenumber, pagesize),
                    FromDate = FromDate,
                    ToDate = ToDate
                };

                return View("History", model);
            });
            }
            else
            {
                var model = new EmployeeLeaveIndexViewModel
                {
                    EmployeeLeaveDetails = null,
                    FromDate = FromDate,
                    ToDate = ToDate
                };
                return View("History", model);
            }
        }

        [Authorize(Roles = "Hr,Admin,Manager")]
        public ActionResult EmployeesLeaveHistory(EmployeeLeaveIndexViewModel model, int pagenumber = 1, int pagesize = 10)
        {
            return Authorization.HasAccess(HttpCurrentUser.EmployeeId, () =>
            {
                model.LeaveStatusList = from leaveStatus in Enum.GetValues(typeof(LeaveStatus)).Cast<LeaveStatus>()
                                        select new SelectListItem
                                        {
                                            Text = _employeeLeaveService.GetEnumsDisplayNameById((int)leaveStatus),
                                            Value = ((int)leaveStatus).ToString()
                                        };
                if (model.FromDate == null || model.ToDate == null)
                {
                    model.LeaveStatusId = (int)Service.Model.LeaveStatus.Apply;
                    return View(model);
                }
                var leaveDetails = new List<EmployeeLeave>();
                if (HttpCurrentUser.IsHR || HttpCurrentUser.IsAdmin)
                {
                    leaveDetails = _employeeLeaveService.GetAllEmployeesLeaveDetails();
                }
                else if (HttpCurrentUser.IsManager)
                {
                    leaveDetails = _employeeLeaveService.GetAllMapppedEmployeesleaveDetails(Convert.ToInt32(HttpCurrentUser.EmployeeId));
                }
                var fromDate = model.FromDate;
                var toDate = model.ToDate;
                leaveDetails = leaveDetails.Where(e =>e.LeaveStatus == model.LeaveStatusId && ((e.LeaveStartDate >= fromDate && e.LeaveStartDate <= toDate)
                                        || (e.LeaveEndDate >= fromDate && e.LeaveEndDate <= toDate)
                                        || (e.LeaveStartDate <= fromDate && e.LeaveEndDate >= toDate))).ToList();

                model.EmployeeLeaveDetails = leaveDetails.ToPagedList(pagenumber, pagesize);
                model.FromDate = fromDate;
                model.ToDate = toDate;

                return View("EmployeesLeaveHistory", model);
            });
        }
        // GET: Leave
        public ActionResult LeavePendingForApproval(int id, int pagenumber = 1, int pagesize = 10)
        {
            return Authorization.HasAccess(Convert.ToString(id), () =>
            {
                var leaveDetails = _employeeLeaveService.GetLeaveDetailsPendingForApproval(id);

                var model = new EmployeeLeaveIndexViewModel
                {
                    EmployeeLeaveDetails = leaveDetails.ToPagedList(pagenumber, pagesize)
                };
                return View(model);
            });
        }

        [Authorize(Roles = "Hr,Admin")]
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
            return Authorization.HasAccess(Convert.ToString(id), () =>
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
                var manager = (from e in _employeeService.GetEmployees()
                               join m in _employeeService.GetEmployees() on e.ReportingManager equals m.EmployeeId
                               where e.EmployeeId.Equals(id)
                               select new
                               {
                                   managerId = e.ReportingManager,
                                   managerName = m.FirstName + " " + m.LastName,
                               }).ToList();
                var model = new EmployeeLeaveViewModel
                {
                    EmployeeLeaveDetails = new EmployeeLeave { EmployeeId = id },
                    LeaveType = leavetype,
                    LeaveStatusEnums = leaveStatus
                    //    LeaveSummaries = _employeeLeaveService.GetLeaveSummary(id)
                };

                var firstOrDefault = manager.FirstOrDefault();
                if (firstOrDefault != null)
                {
                    model.EmployeeLeaveDetails.ApprovedPersonName = firstOrDefault?.managerName ?? "";
                    model.EmployeeLeaveDetails.ApprovedBy = firstOrDefault?.managerId;
                }
                else
                {
                    var hr = GetHr();
                    model.EmployeeLeaveDetails.ApprovedPersonName = hr.FirstOrDefault().FirstName + " " +
                                                                    hr.FirstOrDefault().LastName;
                    model.EmployeeLeaveDetails.ApprovedBy = hr.FirstOrDefault().EmployeeId;
                }
                return View(model);
            });
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(int id, EmployeeLeaveViewModel employeeleave)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    employeeleave.LeaveType = _employeeLeaveService.GetLeaves().Select(l => new SelectListItem
                    {
                        Text = l.Name,
                        Value = l.LeaveTypeId.ToString()
                    });
                    var enumValues = Enum.GetValues(typeof(LeaveStatus)) as LeaveStatus[];
                    employeeleave.LeaveStatusEnums = enumValues?.Select(enumValue => new SelectListItem
                    {

                        Value = ((int)enumValue).ToString(),
                        Text = _employeeLeaveService.GetEnumsNameById(Convert.ToInt32(enumValue))
                    }).ToList();
                    return View(employeeleave);
                }

                var newleave = employeeleave.EmployeeLeaveDetails;
                _employeeLeaveService.AddEmployeeLeaveDetails(newleave);
                var employee = _employeeService.GetEmployee(newleave.EmployeeId);
                var leaveDetails = employeeleave.EmployeeLeaveDetails;
                //Send mail
                if (!ConfigHelper.Environment.ToLower().Equals("dev"))
                {
                    if (!employeeleave.IsSave)
                    {
                        //To Employee
                        SendMailToEmployee(employee, leaveDetails);
                        //To Manager
                        SendMailToManager(employee, leaveDetails);
                        //To Hr
                        //SendMailToHr(employee, leaveDetails);
                    }
                }
                return RedirectToAction("Index/" + id + "");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id, int leaveId)
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
                var employee = _employeeService.GetEmployee(leaveDetails.EmployeeId);
                var leaveTemplate = string.Format("Leave applied from {0} to {1} ",
                employeeleave.EmployeeLeaveDetails.LeaveStartDate.ToShortDateString(),
                employeeleave.EmployeeLeaveDetails.LeaveEndDate.ToShortDateString());

                if (!ConfigHelper.Environment.ToLower().Equals("dev"))
                {
                    //To Employee
                    SendMailToEmployee(employee, leaveDetails);
                    //To Manager
                    SendMailToManager(employee, leaveDetails);
                    //To Hr
                    //SendMailToHr(employee, leaveDetails);
                }
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
            var result = _employeeLeaveService.UpdateLeaveStatus(status, historyid);
            if (result > 0)
            {
                var employeeleave = _employeeLeaveService.GetLeavedetails(historyid);
                var employee = _employeeService.GetEmployee(employeeleave.EmployeeId);
                //To Employee
                SendMailToEmployee(employee, employeeleave);
                //To Manager
                //SendMailToManager(employee, employeeleave);
                //To Hr
                if (Service.Model.LeaveStatus.Approve.ToString().ToLower().Equals(status.ToLower()))
                    SendMailToHr(employee, employeeleave);
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [Authorize(Roles = "Hr,Admin")]
        public ActionResult EmployeeLeaveHistoryReportCsvDownload()
        {
            var leaveDetails = _employeeLeaveService.GetAllEmployeesLeaveDetails()
                .Select(e => new
                {
                    e.EmployeeCode,
                    e.EmployeeName,
                    StartDate = e.LeaveStartDate.ToString("dd-MMM-yyyy"),
                    EndDate = e.LeaveEndDate.ToString("dd-MMM-yyyy"),
                    LeaveType = e.LeaveTypeName,
                    e.NoOfDays,
                    e.Reason,
                    LeaveStatus = e.LeaveStatusDisplayName,
                    LineManager = e.ApprovedPersonName,
                    CreatedDate = e.CreatedDateUTC?.ToString("dd-MMM-yyyy"),
                }).ToList();

            return File(leaveDetails.ToDataTable().ToCsvStream(), "text/csv",
                string.Format("EmployeesLeaveHistory-{0:yyyy-MM-dd-hh-mm-ss}.csv", DateTime.Now));

        }

        public ActionResult GetActualLeaveDaysCount(DateTime? startDate, DateTime? endDate)
        {
            var result = _holidayService.GetActualLeaveDaysCount(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
            return Json(new { Value = result }, JsonRequestBehavior.AllowGet);
        }

        private List<Employee> GetHr()
        {
            var userByHr = RoleManager.FindByName("HR").Users.ToList();
            var employee = _employeeService.GetEmployees();
            var hr = (from u in UserManager.Users.ToList()
                      join ur in userByHr on u.Id equals ur.UserId
                      join e in employee on u.EmployeeId equals e.EmployeeId
                      select new
                      {
                          e.FirstName,
                          e.LastName,
                          u.EmployeeId,
                          e.CompanyEmailId
                      }).ToList();
            return hr.Select(item => new Employee()
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                EmployeeId = item.EmployeeId,
                CompanyEmailId = item.CompanyEmailId
            }).ToList();
        }

        private List<Employee> GetManager()
        {
            var userByHr = RoleManager.FindByName("Manager").Users.ToList();
            var employee = _employeeService.GetEmployees();
            var hr = (from u in UserManager.Users.ToList()
                      join ur in userByHr on u.Id equals ur.UserId
                      join e in employee on u.EmployeeId equals e.EmployeeId
                      select new
                      {
                          e.FirstName,
                          e.LastName,
                          u.EmployeeId,
                          e.CompanyEmailId
                      }).ToList();
            return hr.Select(item => new Employee()
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                EmployeeId = item.EmployeeId,
                CompanyEmailId = item.CompanyEmailId
            }).ToList();
        }

        private void SendMailToEmployee(Employee employee, EmployeeLeave employeeLeave)
        {
            //  var leaveType=e
            var employeeName = employee.FirstName + " " + employee.LastName;
            var employeetemplate = Template.CreateLeaveTemplate(employeeName, CompanyContainer.CompanyName, _employeeLeaveService.GetLeaveTypeNameById(Convert.ToInt32(employeeLeave.LeaveTypeId)), employeeLeave.NoOfDays.ToString(),
                                                employeeLeave.LeaveStartDate.ToString("dd-MMM-yyyy"),
                                                employeeLeave.LeaveEndDate.ToString("dd-MMM-yyyy"), employeeLeave.Reason, _employeeLeaveService.GetEnumsNameById(Convert.ToInt32(employeeLeave.LeaveStatus)));

            SmtpHelper.Send(employee.CompanyEmailId, Subject.Leave, employeetemplate);
        }
        private void SendMailToManager(Employee employee, EmployeeLeave employeeLeave)
        {
            var manager = GetManager().FirstOrDefault(e => (e.FirstName + e.LastName).ToLower() ==
                                                        employeeLeave.ApprovedPersonName.Replace(
                                                            " ", "").ToLower());
            var employeeName = employee.FirstName + " " + employee.LastName;
            var managerName = manager?.FirstName + " " + manager?.LastName;
            var employeetemplate = Template.CreateLeaveTemplate(managerName, employeeName, _employeeLeaveService.GetLeaveTypeNameById(Convert.ToInt32(employeeLeave.LeaveTypeId)), employeeLeave.NoOfDays.ToString(),
                                                employeeLeave.LeaveStartDate.ToString("dd-MMM-yyyy"),
                                                employeeLeave.LeaveEndDate.ToString("dd-MMM-yyyy"), employeeLeave.Reason, _employeeLeaveService.GetEnumsNameById(Convert.ToInt32(employeeLeave.LeaveStatus)));

            SmtpHelper.Send(manager?.CompanyEmailId, Subject.Leave, employeetemplate);
        }
        private void SendMailToHr(Employee employee, EmployeeLeave employeeLeave)
        {
            var employeeName = employee.FirstName + " " + employee.LastName;
            var employeetemplate = Template.CreateLeaveTemplate("", employeeName, _employeeLeaveService.GetLeaveTypeNameById(Convert.ToInt32(employeeLeave.LeaveTypeId)), employeeLeave.NoOfDays.ToString(),
                                          employeeLeave.LeaveStartDate.ToString("dd-MMM-yyyy"),
                                          employeeLeave.LeaveEndDate.ToString("dd-MMM-yyyy"), employeeLeave.Reason, _employeeLeaveService.GetEnumsNameById(Convert.ToInt32(employeeLeave.LeaveStatus)));

            SmtpHelper.Send(ConfigHelper.HrEmail, Subject.Leave, employeetemplate);
        }
    }
}
