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
        public List<LeaveSummary> LeaveSummaries { get; set; }
    }
}