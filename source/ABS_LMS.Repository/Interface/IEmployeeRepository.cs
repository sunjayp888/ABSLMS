using System.Collections.Generic;
using ABS_LMS.Data;

namespace ABS_LMS.Repository.Interface
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IEnumerable<GetAllMappedEmployees_Result> GetAllMappedEmployees(int lineManagerId);
    }
}
