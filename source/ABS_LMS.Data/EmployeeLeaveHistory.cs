//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ABS_LMS.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeLeaveHistory
    {
        public int EmployeeLeaveHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime LeaveStartDate { get; set; }
        public System.DateTime LeaveEndDate { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<int> LeaveTypeId { get; set; }
        public decimal NoOfDays { get; set; }
        public string Reason { get; set; }
        public int LeaveStatus { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateUTC { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDateUTC { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string ApprovedPersonName { get; set; }
        public Nullable<System.DateTime> HalfDayDateUTC { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual LeaveType LeaveType { get; set; }
    }
}
