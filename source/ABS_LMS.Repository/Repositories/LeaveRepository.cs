using System;
using System.Collections.Generic;
using ABS_LMS.Data;
using ABS_LMS.Repository.Interface;

namespace ABS_LMS.Repository.Repositories
{
    public class LeaveRepository: Repository<LeaveDetail>, ILeaveRepository
    {
       public LeaveRepository(ABSLMSEntities context) : base(context)
        {
           }

        public ABSLMSEntities AbsContext => Context as ABSLMSEntities;

        public List<LeaveDetail> GetEmployeeLeaves(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}
