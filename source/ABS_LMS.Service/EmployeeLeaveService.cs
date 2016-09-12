﻿using System;
using System.Collections.Generic;
using System.Linq;
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
                ApprovedPersonName = employeeLeaveDetails.ApprovedPersonName

            });
            _unitOfWork.Complete();
        }

        public List<EmployeeLeave> GetEmployeeLeaveDetails(int employeeId)
        {


            var employeedetails = _unitOfWork.EmployeeLeave.GetEmployeeLeaveDetails(employeeId);
            var leaveTypeList = _unitOfWork.LeaveType.GetAll();

            return employeedetails.Select(employeeLeaveDetails => new EmployeeLeave
            {
                EmployeeLeaveHistoryId = employeeLeaveDetails.EmployeeLeaveHistoryId,
                EmployeeId = employeeLeaveDetails.EmployeeId,
                LeaveStartDate = employeeLeaveDetails.LeaveStartDate,
                LeaveEndDate = employeeLeaveDetails.LeaveEndDate,
                JoiningDate = employeeLeaveDetails.JoiningDate,
                NoOfDays = employeeLeaveDetails.NoOfDays,
                Reason = employeeLeaveDetails.Reason,
                LeaveStatus = employeeLeaveDetails.LeaveStatus,
                ApprovedBy = employeeLeaveDetails.ApprovedBy,
                CreatedDateUTC = employeeLeaveDetails.CreatedDateUTC,
                CreatedBy = employeeLeaveDetails.CreatedBy,
                UpdatedDateUTC = employeeLeaveDetails.UpdatedDateUTC,
                UpdatedBy = employeeLeaveDetails.CreatedBy,
                LeaveTypeId = employeeLeaveDetails.LeaveTypeId,
                LeaveStatusName = GetEnumsNameById(Convert.ToInt32(employeeLeaveDetails.LeaveStatus)),
                LeaveTypeName = leaveTypeList.FirstOrDefault(l => l.LeaveTypeId == employeeLeaveDetails.LeaveTypeId).Name.ToString(),
                ApprovedPersonName = employeeLeaveDetails.ApprovedPersonName



            }).ToList();

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
                NoOfDays = leaveDetails.NoOfDays,
                Reason = leaveDetails.Reason,
                LeaveStatus = leaveDetails.LeaveStatus,
                ApprovedBy = leaveDetails.ApprovedBy,
                CreatedDateUTC = leaveDetails.CreatedDateUTC,
                CreatedBy = leaveDetails.CreatedBy,
                UpdatedDateUTC = leaveDetails.UpdatedDateUTC,
                UpdatedBy = leaveDetails.CreatedBy,
                ApprovedPersonName = leaveDetails.ApprovedPersonName
             
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
            leaveDetails.UpdatedDateUTC = DateTime.UtcNow;
            leaveDetails.CreatedBy = employeeLeaveDetails.UpdatedBy;
            leaveDetails.ApprovedPersonName = employeeLeaveDetails.ApprovedPersonName;
            _unitOfWork.Complete();

        }

        public void UpdateLeaveDetails(int employeeId)
        {
            foreach (var item in _unitOfWork.LeaveType.GetAll())
            {
                _unitOfWork.LeaveDetails.Add(new Data.LeaveDetail
                {

                    EmployeeId = employeeId,
                    LeaveTypeId = item.LeaveTypeId,


                });
            }
            
            _unitOfWork.Complete();

        }

        public List<EmployeeLeave> GetLeaveDetailsByApprovedId(int approvedBy)
        {



            var employeedetails = _unitOfWork.EmployeeLeave.GetLeaveDetailsByApprovedId(approvedBy);
            var leaveTypeList = _unitOfWork.LeaveType.GetAll();
            var employedetails = _unitOfWork.Employee.GetAll().ToList();
            return employeedetails.Select(employeeLeaveDetails => new EmployeeLeave
            {

                EmployeeLeaveHistoryId = employeeLeaveDetails.EmployeeLeaveHistoryId,
                EmployeeId = employeeLeaveDetails.EmployeeId,
                LeaveStartDate = employeeLeaveDetails.LeaveStartDate,
                LeaveEndDate = employeeLeaveDetails.LeaveEndDate,
                JoiningDate = employeeLeaveDetails.JoiningDate,
                NoOfDays = employeeLeaveDetails.NoOfDays,
                Reason = employeeLeaveDetails.Reason,
                LeaveStatus = employeeLeaveDetails.LeaveStatus,
                ApprovedBy = employeeLeaveDetails.ApprovedBy,
                CreatedDateUTC = employeeLeaveDetails.CreatedDateUTC,
                CreatedBy = employeeLeaveDetails.CreatedBy,
                UpdatedDateUTC = employeeLeaveDetails.UpdatedDateUTC,
                UpdatedBy = employeeLeaveDetails.CreatedBy,
                LeaveTypeId = employeeLeaveDetails.LeaveTypeId,
                LeaveStatusName = GetEnumsNameById(Convert.ToInt32(employeeLeaveDetails.LeaveStatus)),
                LeaveTypeName = leaveTypeList.FirstOrDefault(l => l.LeaveTypeId == employeeLeaveDetails.LeaveTypeId)?.Name.ToString(),
                EmployeeName = employedetails.SingleOrDefault(e => e.EmployeeId == employeeLeaveDetails.EmployeeId)?.FirstName + " " + employedetails.SingleOrDefault(e => e.EmployeeId == employeeLeaveDetails.EmployeeId)?.LastName,

                //ApprovedPersonName = employeeLeaveDetails.ApprovedPersonName



            }).ToList();

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
                NoOfDays = employeeLeaveDetails.NoOfDays,
                Reason = employeeLeaveDetails.Reason,
                LeaveStatus = employeeLeaveDetails.LeaveStatus,
                ApprovedBy = employeeLeaveDetails.ApprovedBy,
                CreatedDateUTC = employeeLeaveDetails.CreatedDateUTC,
                CreatedBy = employeeLeaveDetails.CreatedBy,
                UpdatedDateUTC = employeeLeaveDetails.UpdatedDateUTC,
                UpdatedBy = employeeLeaveDetails.CreatedBy,
                LeaveTypeId = employeeLeaveDetails.LeaveTypeId,
                LeaveStatusName = GetEnumsNameById(Convert.ToInt32(employeeLeaveDetails.LeaveStatus)),
                LeaveTypeName = leaveTypeList.FirstOrDefault(l => l.LeaveTypeId == employeeLeaveDetails.LeaveTypeId)?.Name.ToString(),
                EmployeeName = employedetails.SingleOrDefault(e => e.EmployeeId == employeeLeaveDetails.EmployeeId)?.FirstName + " " + employedetails.SingleOrDefault(e => e.EmployeeId == employeeLeaveDetails.EmployeeId)?.LastName,

                //ApprovedPersonName = employeeLeaveDetails.ApprovedPersonName



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

        public string GetEnumsNameById(int enumId)
        {
            LeaveStatus enumDisplayStatus = (LeaveStatus)enumId;
            string stringValue = enumDisplayStatus.ToString();
            return stringValue;
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
    }
}