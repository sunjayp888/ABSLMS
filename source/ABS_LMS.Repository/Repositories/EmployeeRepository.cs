using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ABS_LMS.Data;
using ABS_LMS.Repository.Interface;

namespace ABS_LMS.Repository.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ABSLMSEntities context) : base(context)
        {
        }
        public ABSLMSEntities AbsContext => Context as ABSLMSEntities;

        public IEnumerable<GetAllMappedEmployees_Result> GetAllMappedEmployees(int lineManagerId)
        {
            return AbsContext.GetAllMappedEmployees(lineManagerId);
        }
    }
}
