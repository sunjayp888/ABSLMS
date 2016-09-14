using System.ComponentModel.DataAnnotations;

namespace ABS_LMS.Service.Model
{
    public enum Frequency
    {
        Monthly = 1,
        Querterly = 2,
        HalfYearly = 3,
        Yearly = 4
    }

    public enum EmployeeType
    {
        Confirmed,
        Probation
    }

    public enum LeaveStatus
    {
        [Display(Name = "Draft")]
        Draft =1,
        [Display(Name = "Submitted")]
        Submit =2,
        [Display(Name = "Approved")]
        Approve =3,
        [Display(Name = "Rejected")]
        Reject =4,
        [Display(Name = "Cancelled")]
        Cancel =5
    }
}
