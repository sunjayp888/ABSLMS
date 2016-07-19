using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ABS_LMS.App_Start;
using ABS_LMS.Models.Security;
using ABS_LMS.Service.Interface;

namespace ABS_LMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityWebActivator.Start();
        }

        //protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        //{
        //    if (!Request.IsAuthenticated) return;

        //    var identity = new CustomIdentity(HttpContext.Current.User.Identity);
        //    var principal = new CustomPrincipal(identity);
        //    HttpContext.Current.User = principal;

        //}
    }
}
