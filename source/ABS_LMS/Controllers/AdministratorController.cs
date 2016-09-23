using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ABS_LMS.Models;
//using AnyTime.Helpers;
//using AnyTime.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace ABS_LMS.Controllers
{

   
    public class AdministratorController : Controller
    {
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

        [Authorize(Roles = "Admin")]
        public ActionResult Index(string searchKeyword = null, string sortOrder = null, bool sortOrderDesc = false, int pagenumber = 1, int pagesize = 10)
        {
            ViewBag.SortOrder = !sortOrderDesc;
            searchKeyword = string.IsNullOrEmpty(searchKeyword) ? string.Empty : searchKeyword.Trim().ToLower();
            var users = UserManager.Users;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                searchKeyword = searchKeyword.ToLower().Trim();
                users =
                    users.Where(
                        e =>
                            e.FirstName.Contains(searchKeyword) || e.UserName.Contains(searchKeyword) ||
                            e.LastName.Contains(searchKeyword));
            }
            var model = new AdminUserViewModel
            { 
                Users = !string.IsNullOrEmpty(sortOrder) ? GetSortedUsers(sortOrder, sortOrderDesc, users.ToList()).ToPagedList(pagenumber, pagesize) :
                              users.OrderBy(e => e.UserName)

                             .ToPagedList(pagenumber, pagesize),
                SearchKeyword= searchKeyword


            };
          //  return System.Web.UI.WebControls.View(model);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        private List<ApplicationUser> GetSortedUsers(string sortOrder, bool sortOrderDesc, List<ApplicationUser> users)
        {
            var userList = new List<ApplicationUser>();
            switch (sortOrder.ToLower().Trim())
            {
                case "username":
                    userList = sortOrderDesc ? users.OrderByDescending(s => s.UserName).ToList() : users.OrderBy(s => s.UserName).ToList();
                    break;
                //case "firstname":
                //    userList = sortOrderDesc ? users.OrderByDescending(s => s.FirstName).ToList() : users.OrderBy(s => s.FirstName).ToList();
                //    break;
                //case "lastname":
                //    userList = sortOrderDesc ? users.OrderByDescending(s => s.LastName).ToList() : users.OrderBy(s => s.LastName).ToList();
                //    break;

                case "email":
                    userList = sortOrderDesc ? users.OrderByDescending(s => s.Email).ToList() : users.OrderBy(s => s.Email).ToList();
                    break;
            }
            return userList;
        }



        public async Task<ActionResult> Create()
        {
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmployeeId = 1
                };

                var adminresult = UserManager.Create(user, model.Password);

                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                    return RedirectToAction("Index", "Administrator");
                }
                AddErrors(adminresult);
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.RoleId = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");
            var user = UserManager.FindByName(userName);
            var userRoles = UserManager.GetRoles(user.Id);
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            var model = new AdminUserViewModel
            {
                AspNetUserId = user.Id,
                UserName = userName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
               
                Role = userRoles.FirstOrDefault(),
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            };
            //return System.Web.UI.WebControls.View(model);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AdminUserViewModel model, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByNameAsync(model.UserName).Result;
                user.Email = model.Email;
                //user.FirstName = model.FirstName;
                //user.LastName = model.LastName;
              

                var userRoles = await UserManager.GetRolesAsync(user.Id);
                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                var adminresult = UserManager.Update(user);

                if (adminresult.Succeeded)
                {
                    return RedirectToAction("Index", "Administrator");
                }
                AddErrors(adminresult);
            }
            // ViewBag.Role= UserManager.GetRoles(user.Id);
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            //return System.Web.UI.WebControls.View(model);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(string userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(userId);
            var userRoles = UserManager.GetRoles(user.Id);
            var model = new AdminUserViewModel
            {
                AspNetUserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
              
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            };
            //return System.Web.UI.WebControls.View(model);
            return View(model);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                var NameToUsername = Regex.Replace(error, "Name", "Username");
                ModelState.AddModelError("", NameToUsername);
            }
        }
        [Authorize(Roles = "Admin,User,Manager,Hr")]
        public ActionResult ResetPassword(string userName)
        {
            var model = new ResetPasswordViewModel
            {
                UserName = userName
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = UserManager.FindByName(model.UserName);

            var code = UserManager.GeneratePasswordResetToken(user.Id);
            var result = UserManager.ResetPassword(user.Id, code, model.Password);
            if (result.Succeeded)
            {
                //model.FirstName = user.FirstName;
                //model.LastName = user.LastName;
                model.PasswordMessage = string.Format("Password updated for {0} {1}", user.FirstName, user.LastName);
                return View("ResetPassword", model);
            }
            AddErrors(result);
            return View("ResetPassword", model);
        }

    }
}