using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABS_LMS.Models
{
    public class Announcement
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public byte[] EmployeeImage { get; set; }
        public string Gender { get; set; }
    }
}