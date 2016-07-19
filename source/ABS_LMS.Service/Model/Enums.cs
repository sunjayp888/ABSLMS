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
        Draft=1,
        Submit=2,
        Approve=3,
        Reject=4,
        Cancel=5
    }
}
