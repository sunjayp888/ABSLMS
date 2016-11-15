using System;
using System.Collections.Generic;
using System.Linq;
using ABS_LMS.Data;
using ABS_LMS.Repository.Interface;

namespace ABS_LMS.Repository.Repositories
{
    public class HolidayEntitlementRepository : Repository<HolidayEntitlement>, IHolidayEntitlementRepository
    {
        public HolidayEntitlementRepository(ABSLMSEntities context) : base(context)
        {
        }

        public ABSLMSEntities AbsContext => Context as ABSLMSEntities;

        public List<HolidayEntitlement> GetEmployeeLeaves(int employeeId)
        {
            return AbsContext.HolidayEntitlements.Where(e => e.EmployeeId == employeeId).ToList();
        }
    }
}
