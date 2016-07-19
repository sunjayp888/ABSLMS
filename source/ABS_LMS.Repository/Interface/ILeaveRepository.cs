using System.Collections.Generic;
using ABS_LMS.Data;

namespace ABS_LMS.Repository.Interface
{
    public interface ILeaveRepository:  IRepository<LeaveDetail>
    {
       List<LeaveDetail> GetEmployeeLeaves(int employeeId);
   }
}
