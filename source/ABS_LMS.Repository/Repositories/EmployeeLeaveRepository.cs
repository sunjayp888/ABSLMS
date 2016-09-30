using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS_LMS.Data;
using ABS_LMS.Repository.Interface;

namespace ABS_LMS.Repository.Repositories
{
  public  class EmployeeLeaveRepository : Repository<EmployeeLeaveHistory>, IEmployeeLeaveRepository
    {
        public EmployeeLeaveRepository(ABSLMSEntities context)
            : base(context)
        {
        }
        public ABSLMSEntities AbsContext => Context as ABSLMSEntities;

        public IEnumerable<EmployeeLeaveHistory> GetEmployeeLeaveDetails(int employeeId)
        {
          return AbsContext.EmployeeLeaveHistories.Where(e => e.EmployeeId == employeeId);
        }
        public IEnumerable<EmployeeLeaveHistory> GetLeaveDetailsPendingForApproval(int approvedBy)
        {
            return AbsContext.EmployeeLeaveHistories.Where(e => e.ApprovedBy == approvedBy && e.LeaveStatus == 2);
        }
        public IEnumerable<EmployeeLeaveHistory> GetApprovedLeave()
        {
            return AbsContext.EmployeeLeaveHistories.Where(e => e.LeaveStatus >= 3);
        }

        public IEnumerable<EmployeeLeaveHistory> GetAllEmployeesLeave()
        {
            return AbsContext.EmployeeLeaveHistories;
        }
    }
  
}
