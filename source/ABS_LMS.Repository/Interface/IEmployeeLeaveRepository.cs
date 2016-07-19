using System.Collections.Generic;
using ABS_LMS.Data;

namespace ABS_LMS.Repository.Interface
{
    public interface IEmployeeLeaveRepository : IRepository<EmployeeLeaveHistory>
    {
        IEnumerable<EmployeeLeaveHistory> GetEmployeeLeaveDetails(int employeeId);
        IEnumerable<EmployeeLeaveHistory> GetLeaveDetailsByApprovedId(int approvedBy);
        IEnumerable<EmployeeLeaveHistory> GetApprovedLeave();
    }
}
