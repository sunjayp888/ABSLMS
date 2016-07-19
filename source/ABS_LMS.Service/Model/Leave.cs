using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS_LMS.Service.Model
{
    public class Leave
    {
        public int LeaveDetailsId { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveType { get; set; }
        public string Assigned { get; set; }

    }
}
