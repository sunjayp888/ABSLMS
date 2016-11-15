using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABS_LMS.Models
{
    public class HolidayEntitlement
    {
        public string LeaveType { get; set; }
        public string OpeningBalance { get; set; }
        public string CarryForward { get; set; }
        public string LeaveTaken { get; set; }
        public string Balance { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
    }
}