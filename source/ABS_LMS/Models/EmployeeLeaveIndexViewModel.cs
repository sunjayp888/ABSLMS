using System;
using System.Collections.Generic;
using ABS_LMS.Service.Model;
using PagedList;
using System.Web.Mvc;

namespace ABS_LMS.Models
{
    public class EmployeeLeaveIndexViewModel
    {
        public IPagedList<EmployeeLeave> EmployeeLeaveDetails { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public IEnumerable<SelectListItem> LeaveStatusList { get; set; }
        public int LeaveStatusId { get; set; }
    }
}