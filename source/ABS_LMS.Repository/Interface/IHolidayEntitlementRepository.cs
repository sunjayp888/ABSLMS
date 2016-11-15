using System.Collections.Generic;
using ABS_LMS.Data;

namespace ABS_LMS.Repository.Interface
{
    public interface IHolidayEntitlementRepository:  IRepository<HolidayEntitlement>
    {
       List<HolidayEntitlement> GetEmployeeLeaves(int employeeId);
   }
}
