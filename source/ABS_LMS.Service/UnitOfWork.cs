using System.Collections.Generic;
using ABS_LMS.Data;
using ABS_LMS.Repository.Interface;
using ABS_LMS.Repository.Repositories;
using ABS_LMS.Service.Interface;

namespace ABS_LMS.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ABSLMSEntities _context;

        public UnitOfWork(ABSLMSEntities context)
        {
            _context = context;
            Employee = new EmployeeRepository(_context);
            LeaveDetails = new  LeaveRepository (_context);
            EmployeeLeave = new EmployeeLeaveRepository (_context);
            Department = new DepartmentRepository(_context);
            Designation = new DesignationRepository(_context);
            LeaveType = new LeaveTypeRepository(_context);
            Holiday = new HolidayRepository(_context);
            Event = new EventRepository(_context);
            Client = new ClientRepository(context);
        }

        public IEmployeeRepository Employee { get; private set; }
        public ILeaveRepository LeaveDetails { get; private set; }
        public IEmployeeLeaveRepository EmployeeLeave { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IDesignationRepository Designation { get; private set; }
        public ILeaveTypeRepository LeaveType { get; private set; }
        public IHolidayRepository Holiday { get; private set; }
        public IEventRepository Event { get; private set; }
        public IClientRepository Client { get; private set; }

        //public IEnumerable<GetAllMappedEmployees_Result> GetAllMappedEmployees(int employeeId)
        //{
        //    return _context.GetAllMappedEmployees(employeeId);
        //}

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
