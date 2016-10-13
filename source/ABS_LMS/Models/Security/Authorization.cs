using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;



namespace ABS_LMS.Models.Security
{
    public static class Authorization
    {
        //public static IBusinessService BusinessService { private get; set; }

        private static bool HasAccess(string employeeId) => HttpCurrentUser.IsAdmin || HttpCurrentUser.EmployeeId == employeeId;

        public static ActionResult HasAccess(string employeeId, Func<ActionResult> function)
        {
            if (!HasAccess(employeeId))
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));

            //if (!DoesCurrentRouteControllerMatch("Contract") && BusinessService.HasUnsignedContracts(HttpCurrentUser.ContractorId))
            //    return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Contract", action = "Index", contractorId }));

            return function();
        }

        public static ActionResult HasAccess(string employeeId, Func<ActionResult> function, Func<Exception, ActionResult> exceptionFunction)
        {
            try
            {
                return HasAccess(employeeId, function);
            }
            catch (Exception ex)
            {
                return exceptionFunction(ex);
            }
        }
        private static bool HasAccessEmployee(string employeeId) => HttpCurrentUser.IsAdmin || HttpCurrentUser.IsManager || HttpCurrentUser.IsHR || (HttpCurrentUser.EmployeeId == employeeId && HttpCurrentUser.EmployeeId== employeeId);

        public static ActionResult HasAccessEmployee(string employeeId, Func<ActionResult> function)
        {
            if (!HasAccessEmployee(employeeId))
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));

            //if (!DoesCurrentRouteControllerMatch("Contract") && BusinessService.HasUnsignedContracts(HttpCurrentUser.ContractorId))
            //    return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Contract", action = "Index", contractorId }));

            return function();
        }

        public static ActionResult HasAccessEmployee(string employeeId, Func<ActionResult> function, Func<Exception, ActionResult> exceptionFunction)
        {
            try
            {
                return HasAccessEmployee(employeeId, function);
            }
            catch (Exception ex)
            {
                return exceptionFunction(ex);
            }
        }
        private static bool DoesCurrentRouteControllerMatch(string controller)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            return routeValues["controller"].ToString().Equals(controller);
        }
    }
}
