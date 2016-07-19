using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS_LMS.Data;
using ABS_LMS.Repository.Interface;

namespace ABS_LMS.Repository.Repositories
{
  public  class EmployeeLeaveRepository : Repository<Data.EmployeeLeaveHistory>, IEmployeeLeaveRepository
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
        public IEnumerable<EmployeeLeaveHistory> GetLeaveDetailsByApprovedId(int approvedBy)
        {
            return AbsContext.EmployeeLeaveHistories.Where(e => e.ApprovedBy == approvedBy && (e.LeaveStatus==2 || e.LeaveStatus==3 || e.LeaveStatus==4));
        }
        public IEnumerable<EmployeeLeaveHistory> GetApprovedLeave()
        {
            return AbsContext.EmployeeLeaveHistories.Where(e => e.LeaveStatus >= 3);
        }
    }
  
}
