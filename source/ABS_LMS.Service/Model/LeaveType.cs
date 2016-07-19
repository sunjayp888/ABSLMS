using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS_LMS.Service.Model
{
    public class LeaveType
    {
        public int LeaveTypeId { get; set; }
        public string Name { get; set; }
        public int? Count { get; set; }
        public string Frequency { get; set; }
        public string EmployeeType { get; set; }
    }
}
