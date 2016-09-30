using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS_LMS.Data;
using ABS_LMS.Service.Model;
using LeaveType = ABS_LMS.Service.Model.LeaveType;

namespace ABS_LMS.Service.Interface
{
    public interface IEmployeeLeaveService
    {
        List<EmployeeLeave> GetEmployeeLeaveDetails(int employeeId);
        List<Leave> GetEmployeeLeaveSummary(int employeeId);
        void AddEmployeeLeaveDetails(EmployeeLeave employeeLeaveDetails);
        void UpdateEmployeeLeaveDetails(int leaveDetailsId, EmployeeLeave employeeLeaveDetails);
        EmployeeLeave GetLeavedetails(int leavedetailId);
        List<LeaveType> GetLeaves();
        string GetEnumsNameById(int enumId);
        string GetEnumsDisplayNameById(int enumId);
        int GetEnumsIdByName(string enumName);
        List<EmployeeLeave> GetLeaveDetailsPendingForApproval(int approvedBy);
        List<EmployeeLeave> GetApprovedLeaves();
        int UpdateLeaveStatus(string status, int leaveHistoryId);
        List<LeaveSummary> GetLeaveSummary(int employeeId);
        void UpdateLeaveDetails(int employeeId);
        string GetLeaveTypeNameById(int leaveTypeId);
        List<EmployeeLeave> GetAllMapppedEmployeesleaveDetails(int lineManagerId);
        List<EmployeeLeave> GetAllEmployeesLeaveDetails();
    }
}
