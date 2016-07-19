using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ABS_LMS.Service;
using ABS_LMS.Service.Interface;
using ABS_LMS.Service.Model;
using Microsoft.Practices.Unity;

namespace ABS_LMS.Models
{
    public class EmployeeViewModel
    {
        public Employee EmployeeDetail { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; }

        public IEnumerable<SelectListItem> DesignationList { get; set; }

        public IEnumerable<SelectListItem> EmployeeType { get; set; }
        public IEnumerable<SelectListItem> RoleType { get; set; }

        public IEnumerable<SelectListItem> Gender = new List<SelectListItem>
            {
                new SelectListItem{Text = "Male", Value = "Male"},
                new SelectListItem{Text = "Female", Value = "Female"},
            };
        public IEnumerable<SelectListItem> ReportingManager { get; set; }
    }
}