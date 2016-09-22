using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ABS_LMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
              name: "Administrator",
              url: "Administrator/{action}/{userId}",
              defaults: new { controller = "Administrator", action = "Index" ,userId=UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "Default3",
             url: "Administrator/Edit/{Username}",
             defaults: new { controller = "Administrator", action = "Index" , Username = UrlParameter.Optional}
         );

            routes.MapRoute(
               name: "Default2",
               url: "{controller}/{action}/{id}/{leaveId}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                  name: "Default",
                  url: "{controller}/{action}/{id}",
                  defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
              );
        }
    }
}
