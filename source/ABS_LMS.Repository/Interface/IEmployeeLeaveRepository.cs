using System.Collections.Generic;
using ABS_LMS.Data;

namespace ABS_LMS.Repository.Interface
{
    public interface IEmployeeLeaveRepository : IRepository<EmployeeLeaveHistory>
    {
        IEnumerable<EmployeeLeaveHistory> GetAllEmployeesLeave();
        IEnumerable<EmployeeLeaveHistory> GetEmployeeLeaveDetails(int employeeId);
        IEnumerable<EmployeeLeaveHistory> GetLeaveDetailsPendingForApproval(int approvedBy);
        IEnumerable<EmployeeLeaveHistory> GetApprovedLeave();
    }
}
