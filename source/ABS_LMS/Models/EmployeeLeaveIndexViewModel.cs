using System;
using ABS_LMS.Service.Model;
using PagedList;

namespace ABS_LMS.Models
{
    public class EmployeeLeaveIndexViewModel
    {
        public IPagedList<EmployeeLeave> EmployeeLeaveDetails { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}