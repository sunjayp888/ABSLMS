using System;
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
using PagedList;

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
        [Authorize(Roles = "Admin, Hr")]
        public ActionResult Index(string searchKeyword = "", string sortOrder = null, bool sortOrderDesc = false, int pagenumber = 1, int pagesize = 10)
        {
            ViewBag.SortOrderDesc = !sortOrderDesc;
            ViewBag.SortOrder = sortOrder ?? string.Empty;
            var employees = _employeeService.GetEmployees();
            if (!string.IsNullOrEmpty(searchKeyword))
                employees = employees.Where( e => e.EmployeeCode.Contains(searchKeyword.Trim())
                                            || (e.FirstName + " " + e.LastName).ToLower().Contains(searchKeyword.Trim().ToLower())).ToList();

            var model = new EmployeeIndexViewModel
            {
                SearchKeyword = searchKeyword,
                EmployeeDetail = string.IsNullOrEmpty(sortOrder)
                        ? employees.OrderByDescending(e => e.EmployeeCode).ToPagedList(pagenumber, pagesize)
                        : GetSortedEmployees(sortOrder, sortOrderDesc, employees).ToPagedList(pagenumber, pagesize)
            };
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
        [Authorize(Roles = "Admin, Hr")]
        public ActionResult Create()
        {
            var employeeViewModel = new EmployeeViewModel();
            var model = GetAllDropDownValues(employeeViewModel);
            return View(model);
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeViewModel employee)
        {
            try
            {
                HttpPostedFileBase file = Request.Files["userimage"];
                var getbyte = ConvertToBytes(file);
                employee = GetAllDropDownValues(employee);
                if (!ModelState.IsValid) return View(employee);
                employee.EmployeeDetail.EmployeeImage = getbyte;
                var newemployee = employee.EmployeeDetail;
                _employeeService.AddEmployee(newemployee);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private EmployeeViewModel GetAllDropDownValues(EmployeeViewModel employeeViewModel)
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

            var role = RoleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            employeeViewModel.DepartmentList = departmentlist;
            employeeViewModel.DesignationList = designationslist;
            employeeViewModel.ReportingManager = GetReportingManagerbyId(Convert.ToInt32(departmentlist.FirstOrDefault().Value));
            employeeViewModel.RoleType = role;
            return employeeViewModel;
        }

        // GET: Employee/Edit/5
        [Authorize(Roles = "Admin, Hr")]
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
            TempData["EmployeeImage"] = model.EmployeeDetail.EmployeeImage;

            return View(model);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, EmployeeViewModel employee)
        {
            try
            {
                HttpPostedFileBase file = Request.Files["userimage"];
                var getbyte = ConvertToBytes(file);
                employee = GetAllDropDownValues(employee);
                if (!ModelState.IsValid) return View(employee);
                if (getbyte != null && getbyte.Length > 0)
                {
                    employee.EmployeeDetail.EmployeeImage = getbyte;
                }
                else
                {
                    employee.EmployeeDetail.EmployeeImage = TempData["EmployeeImage"] as byte[];
                }

                _employeeService.UpdateEmployee(id, employee.EmployeeDetail);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        [Authorize(Roles = "Admin, Hr")]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        _employeeService.DeleteEmployee(id);
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        public ActionResult Delete(int id)
        {
            var result = 1;
            _employeeService.DeleteEmployee(id);
            return Json(result, JsonRequestBehavior.DenyGet);
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
                //await UserManager.SendEmailAsync(userName, "Leave Management Reset Password", template);
                SmtpHelper.Send(userName, "LMS Portal Account Created Successfully", template);
            }

            if (!employeeresult.Succeeded)
            {
                ModelState.AddModelError("", employeeresult.Errors.First());
                ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                return View();
            }
            return Json(true, JsonRequestBehavior.DenyGet);
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

        private List<Employee> GetSortedEmployees(string sortOrder, bool sortOrderDesc, IEnumerable<Employee> employees)
        {
            var sortedDocuments = new List<Employee>();
            switch (sortOrder.ToLower().Trim())
            {
                case "employeecode":
                    sortedDocuments = sortOrderDesc ? employees.OrderByDescending(s => s.EmployeeCode).ToList() : employees.OrderBy(s => s.EmployeeCode).ToList();
                    break;
                case "firstname":
                    sortedDocuments = sortOrderDesc ? employees.OrderByDescending(s => s.FirstName).ToList() : employees.OrderBy(s => s.FirstName).ToList();
                    break;
                case "lastname":
                    sortedDocuments = sortOrderDesc ? employees.OrderByDescending(s => s.LastName).ToList() : employees.OrderBy(s => s.LastName).ToList();
                    break;
                case "gender":
                    sortedDocuments = sortOrderDesc ? employees.OrderByDescending(s => s.Gender).ToList() : employees.OrderBy(s => s.Gender).ToList();
                    break;
                case "designation":
                    sortedDocuments = sortOrderDesc ? employees.OrderByDescending(s => s.Designation).ToList() : employees.OrderBy(s => s.Designation).ToList();
                    break;
                case "companyemailid":
                    sortedDocuments = sortOrderDesc ? employees.OrderByDescending(s => s.EmailId).ToList() : employees.OrderBy(s => s.EmailId).ToList();
                    break;
            }
            return sortedDocuments;
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}
