using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABS_LMS.Data;
using ABS_LMS.Helper;
using ABS_LMS.Service;
using ABS_LMS.Service.Interface;
using ABS_LMS.Service.Model;

namespace ABS_LMS.LeaveCalculation
{
    public class LeaveCalculation
    {
        private readonly IEmployeeService _employeeService = new EmployeeService();
        private readonly IEmployeeLeaveService _employeeLeaveService = new EmployeeLeaveService();
        private readonly IHolidayService _holidayService = new HolidayService();


        public bool IsCasualApplicable(EmployeeLeave employeeLeave)
        {
            var employeeData = _employeeService.GetEmployee(employeeLeave.EmployeeId);
            if (employeeData != null && employeeLeave.NoOfDays <= 1 && employeeLeave.LeaveTypeName.ToLower() == LeaveTypeEnum.Casual.ToString().ToLower())
            {
                return employeeData.ConfirmationDateUTC.HasValue &&
                       !IsMondayOrHolidays(employeeLeave.LeaveStartDate) && IsEmployeeHaveCasualLeave(employeeLeave);
            }
            return false;
        }

        public List<Models.HolidayEntitlement> GetEmployeeEntitlement(int employeeId)
        {
            var holidayEntitlementList = new List<Models.HolidayEntitlement>();
            var data = _employeeLeaveService.GetBalanceLeave(employeeId);
            foreach (var item in data)
            {
                holidayEntitlementList.Add(new Models.HolidayEntitlement
                    {
                        Balance = item.Balance.ToString(),
                        CarryForward = item.CarryForward.ToString(),
                        LeaveTaken = item.LeaveTaken.ToString(),
                        LeaveType = item.Name,
                        OpeningBalance = item.OpeningBalance.ToString()
                    }
                    );
            }
            return holidayEntitlementList;
        }

        private bool IsMondayOrHolidays(DateTime startDate)
        {
            var isMondayOrHolidays = startDate.DayOfWeek == DayOfWeek.Monday;
            if (!isMondayOrHolidays)
            {
                //If not monday check whether leave immediately after or before an holiday or weekend
                var holidays = _holidayService.GetAllPublicHolidaysList();
                foreach (var date in holidays)
                {
                    isMondayOrHolidays = (date.Date != null && date.Date.Value.AddDays(-1) == startDate.Date)
                                         || (date.Date != null && date.Date.Value.AddDays(1) == startDate.Date);
                    if (isMondayOrHolidays)
                        break;
                }
            }
            return isMondayOrHolidays;
        }

        private bool IsEmployeeHaveCasualLeave(EmployeeLeave employeeLeave)
        {

            var employeebalanceLeave = _employeeLeaveService.GetEmployeeLeaveDetails(employeeLeave.EmployeeId);
            var currentMonthBalanceLeave = employeebalanceLeave.Where(e => e.LeaveTypeId == (int)LeaveTypeEnum.Casual
                                           && e.LeaveStartDate.Date.Month == employeeLeave.LeaveStartDate.Month)
                .Sum(e => e.NoOfDays);
            return currentMonthBalanceLeave + employeeLeave.NoOfDays <= 2;
        }


    }
}