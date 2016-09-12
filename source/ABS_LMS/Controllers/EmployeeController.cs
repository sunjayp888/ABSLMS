﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABS_LMS.Models;
using ABS_LMS.Service.Interface;
using ABS_LMS.Service.Model;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;
using ABS_LMS.Helper;

namespace ABS_LMS.Controllers
{


    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeLeaveService _employeeLeaveService;
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
        public EmployeeController(IEmployeeService employeeService, IEmployeeLeaveService employeeLeaveService)
        {
            _employeeService = employeeService;
            _employeeLeaveService = employeeLeaveService;
        }

        // GET: Employee
        [Authorize(Roles = "Admin, HR")]
        public ActionResult Index()
        {
            var employees = _employeeService.GetEmployees();
            //var depatmentlist = _employeeService.GetDepartments().Select(d => new SelectListItem
            //{
            //    Text = d.DeparmentName,
            //    Value = d.DepartmentId.ToString()
            //}).ToList();
            //var designationList = _employeeService.GetDesignations().Select(d => new SelectListItem
            //{
            //    Text = d.DesignationName,
            //    Value = d.DesignationId.ToString()
            //}).ToList();
            var model = employees.Select(employee => new EmployeeViewModel
            {
                EmployeeDetail = employee,
                //DepartmentList = depatmentlist,
                //DesignationList=designationList

            });

            return View(model);
        }

        // GET: Employee/Details/5
        [Authorize(Roles = "Admin, HR,User,Manager")]
        public ActionResult Details(int id)
        {
            string reportingManagerName = string.Empty;
            var employee = _employeeService.GetEmployee(id);
            var depatment = _employeeService.GetDepartments().SingleOrDefault(d => d.DepartmentId == employee.DepartmentId);
            var designation = _employeeService.GetDesignations().SingleOrDefault(d => d.DesignationId == employee.DesignationId);
            if (employee.ReportingManager.HasValue)
            {
                reportingManagerName = _employeeService.GetEmployee(Convert.ToInt32(employee.ReportingManager)).FirstName.ToString() + " " + _employeeService.GetEmployee(Convert.ToInt32(employee.ReportingManager)).LastName.ToString();
            }

            employee.Department = depatment.DeparmentName;
            employee.Designation = designation.DesignationName;
            employee.ReportingManagerName = reportingManagerName;
            var model = new EmployeeViewModel
            {
                EmployeeDetail = employee,
            };
            return View(model);
        }

        // GET: Employee/Create
        [Authorize(Roles = "Admin, HR")]
        public ActionResult Create()
        {
            var departmentlist = _employeeService.GetDepartments().Select(d => new SelectListItem
            {
                Text = d.DeparmentName,
                Value = d.DepartmentId.ToString()
            }).ToList();
            var designationslist = _employeeService.GetDesignations().Select(d => new SelectListItem
            {
                Text = d.DesignationName,
                Value = d.DesignationId.ToString()
            }).ToList();


            var model = new EmployeeViewModel
            {
                DepartmentList = departmentlist,
                DesignationList = designationslist,
                ReportingManager = GetReportingManagerbyId(Convert.ToInt32(departmentlist.FirstOrDefault().Value)),

            };
            return View(model);
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeViewModel employee)
        {
            try
            {
                var newemployee = employee.EmployeeDetail;
                _employeeService.AddEmployee(newemployee);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        [Authorize(Roles = "Admin, HR")]
        public ActionResult Edit(int id)
        {

            var employee = _employeeService.GetEmployee(id);
            var aspUser = UserManager.Users.FirstOrDefault(u => u.EmployeeId == id);
            if (aspUser != null)
            {
                var aspNetUser = aspUser.Roles.ToList();
                employee.EmployeeRole = aspNetUser[0].RoleId;
                employee.AspNetUserId = aspNetUser[0].UserId;
            }
            var departmentlist = _employeeService.GetDepartments().Select(d => new SelectListItem
            {
                Text = d.DeparmentName,
                Value = d.DepartmentId.ToString()
            }).ToList();
            var designationslist = _employeeService.GetDesignations().Select(d => new SelectListItem
            {
                Text = d.DesignationName,
                Value = d.DesignationId.ToString()
            }).ToList();

            var role = RoleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();
            var model = new EmployeeViewModel
            {
                EmployeeDetail = employee,
                DepartmentList = departmentlist,
                DesignationList = designationslist,
                ReportingManager = GetReportingManagerbyId(Convert.ToInt32(employee.DepartmentId ?? 0)),
                RoleType = role

            };

            return View(model);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, EmployeeViewModel employee)
        {
            try
            {
                _employeeService.UpdateEmployee(id, employee.EmployeeDetail);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        [Authorize(Roles = "Admin, HR")]
        public ActionResult Delete(int id)
        {
            try
            {
                _employeeService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Employee/Delete/5

        //User method
        public async Task<ActionResult> CreatePortalAccount(int employeeId, string firstName, string lastName, string userName, string userRole)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
                FirstName = firstName,
                LastName = lastName,
                EmployeeId = employeeId
            };
            var password = firstName.ToUpper() + "a@" + Guid.NewGuid().ToString("d").Substring(1, 8);
            var employeeresult = UserManager.Create(user, password);

            if (employeeresult.Succeeded)
            {
                var result = await UserManager.AddToRolesAsync(user.Id, userRole);
                _employeeLeaveService.UpdateLeaveDetails(employeeId);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", employeeresult.Errors.First());
                    ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                    return View();
                }
                var template = Template.PortalAccount(firstName + " " + lastName, userName, password);
              //  await UserManager.SendEmailAsync(userName, "Leave Management Reset Password", template);
                SmtpHelper.Send(userName, "Leave Application", template);
                
            }

            if (!employeeresult.Succeeded)
            {
                ModelState.AddModelError("", employeeresult.Errors.First());
                ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                return View();
            }
            return Json("PortalAccountCreate", JsonRequestBehavior.DenyGet);
        }

        public async Task<ActionResult> UpdateUserRole(string aspNetUserId, string userRole)
        {

            var aspUser = await UserManager.GetRolesAsync(aspNetUserId);
            await UserManager.RemoveFromRoleAsync(aspNetUserId, aspUser[0]);
            await UserManager.AddToRoleAsync(aspNetUserId, userRole);

            return Json("RoleUpdate", JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult GetReportingManager(int departmentId)
        {

            return Json(GetReportingManagerbyId(departmentId));
        }
        private byte[] ConvertIntoByte(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        private IEnumerable<SelectListItem> GetReportingManagerbyId(int departmentId)
        {
            var employee = _employeeService.GetEmployees().Where(e => e.DepartmentId == departmentId).ToList();
            var userByRole = RoleManager.FindByName("Manager").Users.ToList();
            var employeebyUsers = (from u in UserManager.Users.ToList()
                                   join ur in userByRole on u.Id equals ur.UserId
                                   join e in employee on u.EmployeeId equals e.EmployeeId
                                   select new
                                   {
                                       e.FirstName,
                                       e.LastName,
                                       u.EmployeeId
                                   }).ToList();
            var reportingManager = employeebyUsers.Select(m => new SelectListItem
            {
                Text = m.FirstName + " " + m.LastName,
                Value = m.EmployeeId.ToString(),
            }).ToList();
            var hr = GetHr();
            if (hr.Any())
            {
                reportingManager.AddRange(GetHr().Select(m => new SelectListItem
                {
                    Text = m.FirstName + " " + m.LastName,
                    Value = m.EmployeeId.ToString(),
                }).ToList());
            }
            return reportingManager;
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

    }
}