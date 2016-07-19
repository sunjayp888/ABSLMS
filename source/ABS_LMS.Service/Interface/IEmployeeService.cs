using System.Collections.Generic;
using ABS_LMS.Service.Model;

namespace ABS_LMS.Service.Interface
{
    public interface IEmployeeService
    {
        Employee GetEmployee(int employeeId);
        List<Employee> GetEmployees();
        void AddEmployee(Employee employee);
        void UpdateEmployee(int employeeId, Employee employee);
        List<Department> GetDepartments();
        List<Designation> GetDesignations();
       void DeleteEmployee(int employeeId);
    }
}
