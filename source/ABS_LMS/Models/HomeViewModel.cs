using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABS_LMS.Service.Model;

namespace ABS_LMS.Models
{
    public class HomeViewModel
    {
        public List<Employee> EmployeeBirthday { get; set; }
        public List<Event> Events { get; set; }
        public List<Announcement> Announcements { get; set; }
    }
}