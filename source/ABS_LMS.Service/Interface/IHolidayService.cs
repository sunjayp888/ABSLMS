using System;
using System.Collections.Generic;
using ABS_LMS.Service.Model;

namespace ABS_LMS.Service.Interface
{
    public interface IHolidayService
    {
        List<Holiday> GetAllPublicHolidaysList(); 
        int GetActualLeaveDaysCount(DateTime startDate, DateTime endDate);
        void AddHoliday(Holiday holiday);
        void DeleteHoliday(int holidayId);
    }
}
