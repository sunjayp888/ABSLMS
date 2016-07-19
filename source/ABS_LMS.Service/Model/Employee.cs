﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ABS_LMS.Service.Model
{
    public class Employee
    {
        [DisplayName("Employee Id")]
        public int EmployeeId { get; set; }

        [DisplayName("Employee Code")]
        [Required(ErrorMessage = "Please Enter Employee Code")]
        public string EmployeeCode { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        [DisplayName("Personal Email Id")]
        [Required(ErrorMessage = "Please Enter Personal Email Id")]
        public string EmailId { get; set; }

        [DisplayName("Company Email Id")]
        public string CompanyEmailId { get; set; }

        [DisplayName("Mobile Number")]
        [Required(ErrorMessage = "Please Enter Mobile Number")]
        public string MobileNo { get; set; }

        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "Please Enter Date Of Joining")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DOJ { get; set; }

        [DisplayName("Department")]
        public int? DepartmentId { get; set; }

        [DisplayName("Designation")]
        public int? DesignationId { get; set; }

        public string Department { get; set; }
        public string Designation { get; set; }

        [DisplayName("Leaving Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? LeavingDateUTC { get; set; }

        //[DisplayName("Photo")]
        //public string Photo_Byte_ { get; set; }

        public string PANCard { get; set; }

        [DisplayName("Aadhar Id")]
        public string AadharId { get; set; }

        [DisplayName("Confirmation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ConfirmationDateUTC { get; set; }

        [DisplayName("Leave Details")]
        public virtual ICollection<Leave> LeaveDetails { get; set; }

        public string Gender { get; set; }

        [DisplayName("Reporting Manager")]
        public int ? ReportingManager { get; set; }

        [DisplayName("Reporting Manager")]
        public string ReportingManagerName { get; set; }

        [DisplayName("Role")]
        public string EmployeeRole { get; set; }

        
        public string AspNetUserId { get; set; }
        
         public string DateofBirth
        {
            get { return DOB.GetValueOrDefault().ToShortDateString(); }
        }
    }

}
