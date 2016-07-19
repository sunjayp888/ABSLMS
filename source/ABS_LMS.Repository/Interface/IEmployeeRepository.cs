using System.Collections.Generic;
using ABS_LMS.Data;

namespace ABS_LMS.Repository.Interface
{
    public interface IEmployeeRepository : IRepository<Employee>
    {

        ////select from employee
        //List<Employee> GetEmployees();
        //Employee GetEmployeeDetails(int employeeId);
        //List<Attendance> getEmployeesAttendance(int Id);
        //List<DepartmentRepository> getAllDepartment();
        //List<JobDescription> getAlljobDescription();
        //List<EmployeeLeave> getLeaveRequest(int employeeId);

        ////insert into employee
        //void InsertIntoEmployee(AllEmployeeDetails Emp);
        //void InsertIntoEmployeeLeave(EmployeeLeave Empleave);

        //////update into employee
        //void EmployeeLogin(string Username, string Password);

        //AllEmployeeDetails getEmployee(int Id);

        //void getEmployeeAttendance(int Id);

        //void getEmployeeLeave(int Id);

        //void updateEmployee(AllEmployeeDetails Emp);
        //AllEmployeeDetails getNewemployee(int Id);

        //EmployeeLeave getLeave(int Id);
    }
}
