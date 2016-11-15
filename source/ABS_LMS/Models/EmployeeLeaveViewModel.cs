using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using ABS_LMS.Service.Model;

namespace ABS_LMS.Models
{
    public class EmployeeLeaveViewModel
    {
        public EmployeeLeave EmployeeLeaveDetails { get; set; }
        public IEnumerable<SelectListItem> LeaveType { get; set; }
        public IEnumerable<SelectListItem> LeaveStatusEnums { get; set; }
        public List<EntitlementSummary> LeaveSummaries { get; set; }
        public bool IsSave { get; set; }
        public bool IsCasualApplicable { get; set; }
        public List<HolidayEntitlement> HolidayEntitlements { get; set; }
    }
}