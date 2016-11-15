using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ABS_LMS.Data;
using ABS_LMS.Service.Interface;
using ABS_LMS.Service.Model;
using LeaveType = ABS_LMS.Service.Model.LeaveType;

namespace ABS_LMS.Service
{
    public class EmployeeLeaveService : IEmployeeLeaveService
    {
        private readonly UnitOfWork _unitOfWork;

        public EmployeeLeaveService()
        {
            _unitOfWork = new UnitOfWork(new ABSLMSEntities(ConfigHelper.ConnectionString));
        }

        public void AddEmployeeLeaveDetails(EmployeeLeave employeeLeaveDetails)
        {
            _unitOfWork.EmployeeLeave.Add(new Data.EmployeeLeaveHistory
            {
                EmployeeId = employeeLeaveDetails.EmployeeId,
                LeaveStartDate = employeeLeaveDetails.LeaveStartDate,
                LeaveEndDate = employeeLeaveDetails.LeaveEndDate,
                JoiningDate = employeeLeaveDetails.JoiningDate,
                NoOfDays = employeeLeaveDetails.NoOfDays,
                Reason = employeeLeaveDetails.Reason,
                LeaveStatus = employeeLeaveDetails.LeaveStatus,
                LeaveTypeId = employeeLeaveDetails.LeaveTypeId,
                ApprovedBy = employeeLeaveDetails.ApprovedBy,
                CreatedDateUTC = DateTime.UtcNow,
                CreatedBy = employeeLeaveDetails.CreatedBy,
                UpdatedDateUTC = DateTime.UtcNow,
                UpdatedBy = employeeLeaveDetails.CreatedBy,
                ApprovedPersonName = employeeLeaveDetails.ApprovedPersonName,
                HalfDayDateUTC =  employeeLeaveDetails.HalfDayDateUTC 
            });
            _unitOfWork.Complete();
        }

        public List<EmployeeLeave> GetEmployeeLeaveDetails(int employeeId)
        {   
            var employeeLeaveDetails = _unitOfWork.EmployeeLeave.GetEmployeeLeaveDetails(employeeId).ToList();
           
            return employeeLeaveDetails.Select(leaveDetails => new EmployeeLeave
            {
                EmployeeLeaveHistoryId = leaveDetails.EmployeeLeaveHistoryId,
                EmployeeId = leaveDetails.EmployeeId,
                LeaveStartDate = leaveDetails.LeaveStartDate,
                LeaveEndDate = leaveDetails.LeaveEndDate,
                JoiningDate = leaveDetails.JoiningDate,
                NoOfDays = Math.Round(leaveDetails.NoOfDays, 1),
                Reason = leaveDetails.Reason,
                LeaveStatus = leaveDetails.LeaveStatus,
                ApprovedBy = leaveDetails.ApprovedBy,
                CreatedDateUTC = leaveDetails.CreatedDateUTC,
                CreatedBy = leaveDetails.CreatedBy,
                UpdatedDateUTC = leaveDetails.UpdatedDateUTC,
                UpdatedBy = leaveDetails.CreatedBy,
                LeaveTypeId = leaveDetails.LeaveTypeId,
                LeaveStatusName = GetEnumsNameById(Convert.ToInt32(leaveDetails.LeaveStatus)),
                LeaveStatusDisplayName = GetEnumsDisplayNameById(Convert.ToInt32(leaveDetails.LeaveStatus)),
                LeaveTypeName = leaveDetails.LeaveType.Name,
                EmployeeName = leaveDetails.Employee.FirstName + " " + leaveDetails.Employee.LastName,
                EmployeeCode = leaveDetails.Employee.EmployeeCode,
                ApprovedPersonName = leaveDetails.ApprovedPersonName,
                HalfDayDateUTC = leaveDetails.HalfDayDateUTC
            }).OrderByDescending(o => o.CreatedDateUTC).ToList();
        }
        
        public List<Leave> GetEmployeeLeaveSummary(int employeeId)
        {
            throw new NotImplementedException();
        }

        public EmployeeLeave GetLeavedetails(int leavedetailId)
        {
            var leaveDetails = _unitOfWork.EmployeeLeave.Get(leavedetailId);
            return new EmployeeLeave
            {
                EmployeeLeaveHistoryId = leaveDetails.EmployeeLeaveHistoryId,
                EmployeeId = leaveDetails.EmployeeId,
                LeaveStartDate = leaveDetails.LeaveStartDate,
                LeaveEndDate = leaveDetails.LeaveEndDate,
                JoiningDate = leaveDetails.JoiningDate,
                NoOfDays = Math.Round(leaveDetails.NoOfDays, 1),
                Reason = leaveDetails.Reason,
                LeaveStatus = leaveDetails.LeaveStatus,
                ApprovedBy = leaveDetails.ApprovedBy,
                CreatedDateUTC = leaveDetails.CreatedDateUTC,
                CreatedBy = leaveDetails.CreatedBy,
                UpdatedDateUTC = leaveDetails.UpdatedDateUTC,
                UpdatedBy = leaveDetails.CreatedBy,
                ApprovedPersonName = leaveDetails.ApprovedPersonName,
                LeaveTypeId = leaveDetails.LeaveTypeId,
                HalfDayDateUTC = leaveDetails.HalfDayDateUTC

            };
        }

        public List<Model.LeaveType> GetLeaves()
        {
            return _unitOfWork.LeaveType.GetAll().Select(e => new LeaveType
            {
                LeaveTypeId = e.LeaveTypeId,
                Name = e.Name,
                EmployeeType = e.EmployeeType,
                Frequency = e.Frequency
            }).ToList();
        }

        public void UpdateEmployeeLeaveDetails(int leaveDetailsId, EmployeeLeave employeeLeaveDetails)
        {
            var leaveDetails = _unitOfWork.EmployeeLeave.Get(leaveDetailsId);
            leaveDetails.LeaveStartDate = employeeLeaveDetails.LeaveStartDate;
            leaveDetails.LeaveEndDate = employeeLeaveDetails.LeaveEndDate;
            leaveDetails.JoiningDate = employeeLeaveDetails.JoiningDate;
            leaveDetails.NoOfDays = employeeLeaveDetails.NoOfDays;
            leaveDetails.Reason = employeeLeaveDetails.Reason;
            leaveDetails.LeaveStatus = employeeLeaveDetails.LeaveStatus;
            leaveDetails.LeaveTypeId = employeeLeaveDetails.LeaveTypeId;
            leaveDetails.UpdatedDateUTC = DateTime.UtcNow;
            leaveDetails.CreatedBy = employeeLeaveDetails.UpdatedBy;
            leaveDetails.ApprovedPersonName = employeeLeaveDetails.ApprovedPersonName;
            leaveDetails.HalfDayDateUTC = employeeLeaveDetails.HalfDayDateUTC;
            _unitOfWork.Complete();

        }

        public void UpdateLeaveDetails(int employeeId)
        {
            foreach (var item in _unitOfWork.LeaveType.GetAll())
            {
                _unitOfWork.LeaveDetails.Add(new Data.LeaveDetail
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = item.LeaveTypeId
                });
            }           
            _unitOfWork.Complete();
        }

        public List<EmployeeLeave> GetLeaveDetailsPendingForApproval(int approvedBy)
        {

            var employeedetails = _unitOfWork.EmployeeLeave.GetLeaveDetailsPendingForApproval(approvedBy);
            var leaveTypeList = _unitOfWork.LeaveType.GetAll();
            var employedetails = _unitOfWork.Employee.GetAll().ToList();
            return employeedetails.Select(employeeLeaveDetails => new EmployeeLeave
            {

                EmployeeLeaveHistoryId = employeeLeaveDetails.EmployeeLeaveHistoryId,
                EmployeeId = employeeLeaveDetails.EmployeeId,
                LeaveStartDate = employeeLeaveDetails.LeaveStartDate,
                LeaveEndDate = employeeLeaveDetails.LeaveEndDate,
                JoiningDate = employeeLeaveDetails.JoiningDate,
                NoOfDays = Math.Round(employeeLeaveDetails.NoOfDays, 1),
                Reason = employeeLeaveDetails.Reason,
                LeaveStatus = employeeLeaveDetails.LeaveStatus,
                ApprovedBy = employeeLeaveDetails.ApprovedBy,
                CreatedDateUTC = employeeLeaveDetails.CreatedDateUTC,
                CreatedBy = employeeLeaveDetails.CreatedBy,
                UpdatedDateUTC = employeeLeaveDetails.UpdatedDateUTC,
                UpdatedBy = employeeLeaveDetails.CreatedBy,
                LeaveTypeId = employeeLeaveDetails.LeaveTypeId,
                LeaveStatusDisplayName = GetEnumsDisplayNameById(Convert.ToInt32(employeeLeaveDetails.LeaveStatus)),
                LeaveStatusName = GetEnumsNameById(Convert.ToInt32(employeeLeaveDetails.LeaveStatus)),
                LeaveTypeName = leaveTypeList.FirstOrDefault(l => l.LeaveTypeId == employeeLeaveDetails.LeaveTypeId)?.Name.ToString(),
                EmployeeName = employedetails.SingleOrDefault(e => e.EmployeeId == employeeLeaveDetails.EmployeeId)?.FirstName + " " + employedetails.SingleOrDefault(e => e.EmployeeId == employeeLeaveDetails.EmployeeId)?.LastName,
                //ApprovedPersonName = employeeLeaveDetails.ApprovedPersonName
                HalfDayDateUTC = employeeLeaveDetails.HalfDayDateUTC

            }).OrderByDescending(o => o.CreatedDateUTC).ToList();

        }
        public List<EmployeeLeave> GetApprovedLeaves()
        {
            var employeedetails = _unitOfWork.EmployeeLeave.GetApprovedLeave();
            var leaveTypeList = _unitOfWork.LeaveType.GetAll();
            var employedetails = _unitOfWork.Employee.GetAll().ToList();
            return employeedetails.Select(employeeLeaveDetails => new EmployeeLeave
            {
                EmployeeLeaveHistoryId = employeeLeaveDetails.EmployeeLeaveHistoryId,
                EmployeeId = employeeLeaveDetails.EmployeeId,
                LeaveStartDate = employeeLeaveDetails.LeaveStartDate,
                LeaveEndDate = employeeLeaveDetails.LeaveEndDate,
                JoiningDate = employeeLeaveDetails.JoiningDate,
                NoOfDays = Math.Round(employeeLeaveDetails.NoOfDays, 1),
                Reason = employeeLeaveDetails.Reason,
                LeaveStatus = employeeLeaveDetails.LeaveStatus,
                ApprovedBy = employeeLeaveDetails.ApprovedBy,
                CreatedDateUTC = employeeLeaveDetails.CreatedDateUTC,
                CreatedBy = employeeLeaveDetails.CreatedBy,
                UpdatedDateUTC = employeeLeaveDetails.UpdatedDateUTC,
                UpdatedBy = employeeLeaveDetails.CreatedBy,
                LeaveTypeId = employeeLeaveDetails.LeaveTypeId,
                LeaveStatusDisplayName = GetEnumsDisplayNameById(Convert.ToInt32(employeeLeaveDetails.LeaveStatus)),
                LeaveStatusName = GetEnumsNameById(Convert.ToInt32(employeeLeaveDetails.LeaveStatus)),
                LeaveTypeName = leaveTypeList.FirstOrDefault(l => l.LeaveTypeId == employeeLeaveDetails.LeaveTypeId)?.Name.ToString(),
                EmployeeName = employedetails.SingleOrDefault(e => e.EmployeeId == employeeLeaveDetails.EmployeeId)?.FirstName + " " + employedetails.SingleOrDefault(e => e.EmployeeId == employeeLeaveDetails.EmployeeId)?.LastName,
                //ApprovedPersonName = employeeLeaveDetails.ApprovedPersonName
                HalfDayDateUTC = employeeLeaveDetails.HalfDayDateUTC
            }).ToList();

        }
        public int UpdateLeaveStatus(string status, int leaveHistoryId)
        {
            var leaveDetails = _unitOfWork.EmployeeLeave.Get(leaveHistoryId);
            leaveDetails.LeaveStatus = GetEnumsIdByName(status);
            return _unitOfWork.Complete();

        }


        public List<LeaveSummary> GetLeaveSummary(int employeeId)
        {

            var leaveSummary = from lt in _unitOfWork.LeaveType.GetAll().ToList()
                               join ld in _unitOfWork.LeaveDetails.GetAll().ToList().Where(l=>l.EmployeeId==employeeId) on new { LeaveTypeId = lt.LeaveTypeId } equals
                                   new { LeaveTypeId = ld.LeaveTypeId } into ld_join
                               from ld in ld_join.DefaultIfEmpty()
                               join e in _unitOfWork.Employee.GetAll().ToList().Where(e => e.EmployeeId == employeeId) on ld.EmployeeId equals e.EmployeeId into e_join
                               from e in e_join.DefaultIfEmpty()
                               where
           ld.EmployeeId == employeeId
                               select new LeaveSummary
                               {
                                   LeaveTypeId = (int?)lt.LeaveTypeId,
                                   Name = lt.Name,
                                   Count = lt.Count,
                                   Frequency = lt.Frequency,
                                   EmployeeType = lt.EmployeeType,
                                   Total = (decimal?)Convert.ToDecimal((
                                    lt.Name == "Earned" &&
  e.ConfirmationDateUTC != null ? (lt.Count * (
 Convert.ToDateTime(DateTime.Now).Year != Convert.ToDateTime(Convert.ToDateTime(e.ConfirmationDateUTC)).Year ? (System.Decimal)Convert.ToDateTime(DateTime.Now).Month : (Convert.ToDateTime(DateTime.Now).Month - Convert.ToDateTime(Convert.ToDateTime(e.ConfirmationDateUTC)).Month + (30 - Convert.ToDateTime(Convert.ToDateTime(e.ConfirmationDateUTC)).Day * lt.Count) / 30))) :
  lt.Name == "Casual" &&
                                             e.ConfirmationDateUTC != null
                                               ? lt.Count
                                               : lt.Name == "Sick" ? lt.Count : 0)),
                                   LeaveTaken = ((System.Decimal?)ld.LeaveTaken ?? (System.Decimal?)0),
                                   CarryForward = ((System.Decimal?)ld.CarryForward ?? (System.Decimal?)0),
                                   Balance = ((System.Decimal?)ld.Balance ?? (System.Decimal?)0)
                               };

            return leaveSummary.ToList();
        }
        public string GetLeaveTypeNameById(int leaveTypeId)
        {
            var LeaveTypeName = _unitOfWork.LeaveType.GetAll().FirstOrDefault(s => s.LeaveTypeId == leaveTypeId).Name;
            return LeaveTypeName;
        }

        public string GetEnumsNameById(int enumId)
        {
            LeaveStatus enumDisplayStatus = (LeaveStatus)enumId;
            string stringValue = enumDisplayStatus.ToString();
            return stringValue;
        }

        public string GetEnumsDisplayNameById(int enumId)
        {
            var enumLeaveStatus = (LeaveStatus)enumId;
            var fieldInfo = enumLeaveStatus.GetType().GetField(enumLeaveStatus.ToString());
            var attrs = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = "";
            if (!attrs.Any()) return outString;
            var displayAttr = ((DisplayAttribute)attrs[0]);
            outString = displayAttr.Name;
            return outString;
        }

        public int GetEnumsIdByName(string enumName)
        {
            LeaveStatus id = (LeaveStatus)Enum.Parse(typeof(LeaveStatus), enumName, true); ;
            //LeaveStatus enumDisplayStatus;
            //enumDisplayStatus.CompareTo(enumName)
            //int stringValue = enumDisplayStatus.ToString();
            //return stringValue;
            return (int)id;
        }

        public List<EmployeeLeave> GetAllMapppedEmployeesleaveDetails(int lineManagerId)
        {
            var employeeLeaves = new List<EmployeeLeave>();
            var allMappedEmployees = _unitOfWork.Employee.GetAllMappedEmployees(lineManagerId).ToList();
            foreach (var emp in allMappedEmployees)
            {
                employeeLeaves.AddRange(GetEmployeeLeaveDetails(emp.EmployeeID??0));
            }
            return employeeLeaves.OrderByDescending(r => r.EmployeeId).ToList();
        }

        public List<EmployeeLeave> GetAllEmployeesLeaveDetails()
        {
            var employeeLeaveDetails = _unitOfWork.EmployeeLeave.GetAllEmployeesLeave().ToList();
            return employeeLeaveDetails.Select(leaveDetails => new EmployeeLeave
            {
                EmployeeLeaveHistoryId = leaveDetails.EmployeeLeaveHistoryId,
                EmployeeId = leaveDetails.EmployeeId,
                LeaveStartDate = leaveDetails.LeaveStartDate,
                LeaveEndDate = leaveDetails.LeaveEndDate,
                JoiningDate = leaveDetails.JoiningDate,
                NoOfDays = Math.Round(leaveDetails.NoOfDays, 1),
                Reason = leaveDetails.Reason,
                LeaveStatus = leaveDetails.LeaveStatus,
                ApprovedBy = leaveDetails.ApprovedBy,
                CreatedDateUTC = leaveDetails.CreatedDateUTC,
                CreatedBy = leaveDetails.CreatedBy,
                UpdatedDateUTC = leaveDetails.UpdatedDateUTC,
                UpdatedBy = leaveDetails.CreatedBy,
                LeaveTypeId = leaveDetails.LeaveTypeId,
                LeaveStatusDisplayName = GetEnumsDisplayNameById(Convert.ToInt32(leaveDetails.LeaveStatus)),
                LeaveStatusName = GetEnumsNameById(Convert.ToInt32(leaveDetails.LeaveStatus)),
                LeaveTypeName = leaveDetails.LeaveType.Name,
                EmployeeName = leaveDetails.Employee.FirstName + " " + leaveDetails.Employee.LastName,
                EmployeeCode = leaveDetails.Employee.EmployeeCode,
                ApprovedPersonName = leaveDetails.ApprovedPersonName,
                HalfDayDateUTC = leaveDetails.HalfDayDateUTC
            }).ToList();
        }
    }
}
