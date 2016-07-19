using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS_LMS.Service.Model
{
    public  class LeaveSummary
    {
        public int? LeaveTypeId { get; set; }
        public string Name { get; set; }
        public decimal? Count { get; set; }
        public string Frequency { get; set; }
        public string EmployeeType { get; set; }
        public decimal? Total { get; set; }
        public decimal? LeaveTaken { get; set; }
        public decimal? CarryForward { get; set; }
        public decimal? Balance { get; set; }
    }
}
