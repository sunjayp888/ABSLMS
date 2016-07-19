using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ABS_LMS.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace ABS_LMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name)
            : base(name)
        {
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base(ConfigHelper.SqlConnectionString, throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
            //Database.SetInitializer<ApplicationDbContext>(new CustomInitializer());
            //Create().Database.Initialize(true);
        }

        public class CustomInitializer : IDatabaseInitializer<ApplicationDbContext>
        {
            public void InitializeDatabase(ApplicationDbContext context)
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

                if (!userManager.Users.Any(u => u.UserName == "admin"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "admin",
                        //FirstName = "admin"
                    };

                    var roleId = roleManager.Roles.FirstOrDefault(r => r.Name == "Admin").Id;
                    user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = roleId });

                    userManager.Create(user, "Admin123!");
                }
            }
        }
    }
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore)
            : base(roleStore)
        { }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));
        }


    }
}