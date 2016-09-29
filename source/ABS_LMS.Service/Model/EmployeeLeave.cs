using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace ABS_LMS.Service.Model
{
    public class EmployeeLeave
    {
        public int EmployeeLeaveHistoryId { get; set; }

        [DisplayName("Employee Id")]
        public int EmployeeId { get; set; }

        [DisplayName("Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        [Required(ErrorMessage = "Please Enter Leave Start Date")]
        [DataType(DataType.Date)]
        public DateTime LeaveStartDate { get; set; }

        [DisplayName("End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        [Required(ErrorMessage = "Please Enter Leave End Date")]
        [DataType(DataType.Date)]
        public DateTime LeaveEndDate { get; set; }

        [DisplayName("Joining Date")]
        public DateTime? JoiningDate { get; set; }

        [DisplayName("Leave Type")]
        public int? LeaveTypeId { get; set; }

        public string LeaveTypeName { get; set; }

        [DisplayName("No Of Days")]
        //[DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal NoOfDays { get; set; }

        [DataType(DataType.MultilineText)]
        public string Reason { get; set; }

        [DisplayName("Leave Status")]
        public int LeaveStatus { get; set; }
        public string LeaveStatusName { get; set; }
        public string LeaveStatusDisplayName { get; set; }

        [DisplayName("Line Manager")]
        public int? ApprovedBy { get; set; }
        public string ApprovedPersonName { get; set; }

        [DisplayName("Created Date")]
        public DateTime? CreatedDateUTC { get; set; }

        [DisplayName("Created By")]
        public int? CreatedBy { get; set; }

        [DisplayName("Updated Date")]
        public DateTime? UpdatedDateUTC { get; set; }

        [DisplayName("Updated By")]
        public int? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual LeaveType LeaveType { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        public bool IsHalfDay
        {
            get { return HalfDayDateUTC.HasValue; }
        }

        [DisplayName("HalfDay Date")]
        public DateTime? HalfDayDateUTC { get; set; }
    }
}
