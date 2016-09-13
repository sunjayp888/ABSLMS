using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using ABS_LMS;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace ABS_LMS.Models.Security
{
    public class HttpCurrentUser
    {
      
        // this usermanager we are getting is created once per request and stored in the OwinContext
        //http://blogs.msdn.com/b/webdev/archive/2014/02/12/per-request-lifetime-management-for-usermanager-class-in-asp-net-identity.aspx
        private static ApplicationUserManager UserManager => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public static bool IsAdmin => ((ClaimsPrincipal)HttpContext.Current.User).IsInRole("Admin");
        public static bool IsUser => ((ClaimsPrincipal)HttpContext.Current.User).IsInRole("User");
        public static bool IsManager => ((ClaimsPrincipal)HttpContext.Current.User).IsInRole("Manager");
        public static bool IsHR => ((ClaimsPrincipal)HttpContext.Current.User).IsInRole("Hr");

        public static string Name => HttpContext.Current.User?.Identity.Name;
        public static string Id => UserManager.Users.FirstOrDefault(e => e.UserName == Name)?.Id;

        public static string FirstName => UserManager.Users.FirstOrDefault(e => e.UserName == Name)?.FirstName;

        public static string LastName => UserManager.Users.FirstOrDefault(e => e.UserName == Name)?.LastName;

        public static string EmployeeId => UserManager.Users.FirstOrDefault(e => e.UserName == Name)?.EmployeeId.ToString();

        public static string RoleName => UserManager.GetRoles(Id).FirstOrDefault();



    }
}