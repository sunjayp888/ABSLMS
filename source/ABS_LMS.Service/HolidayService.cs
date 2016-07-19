using System;
using System.Linq;
using ABS_LMS.Data;
using ABS_LMS.Service.Interface;
using ABS_LMS.Service.Model;

namespace ABS_LMS.Service
{
    public class HolidayService : IHolidayService
    {
        private readonly UnitOfWork _unitOfWork;
        private const string PublicHoliday = "H";
        private const string WeekEnd = "W";

        public HolidayService()
        {
            _unitOfWork = new UnitOfWork(new ABSLMSEntities(ConfigHelper.ConnectionString));
        }

        public int GetActualLeaveDaysCount(DateTime startDate, DateTime endDate)
        {
            #region calculation Done
            //int totalDaysCount = Convert.ToInt32((endDate - startDate).TotalDays + 1);
            //int holidayCount = _unitOfWork.Holiday.GetAll().Count(h => h.Type == PublicHoliday && h.Date >= startDate && h.Date <= endDate);
            //var newdates = Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => new { Date = startDate.AddDays(d), Day = startDate.AddDays(d).DayOfWeek });
            //int weekEndDays = newdates.AsEnumerable()
            //    .Join(_unitOfWork.Holiday.GetAll().Where(h => h.Type == WeekEnd),
            //    r => r.Day.ToString().ToLower(),
            //    d => d.Day.ToString().ToLower(),
            //    (r, d) => d).ToList().Count;
            //return totalDaysCount - weekEndDays - holidayCount;
            #endregion calculation Done

            #region New Calculation Done

            var dates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                    .Select(d => new {Date = startDate.AddDays(d), Day = startDate.AddDays(d).DayOfWeek}).ToList();

            return
                dates.Count
                - dates.Join(_unitOfWork.Holiday.GetAll().Where(h => h.Type == WeekEnd),
                    r => r.Day.ToString().ToLower(),
                    d => d.Day.ToString().ToLower(),
                    (r, d) => d).ToList().Count
                -
                _unitOfWork.Holiday.GetAll()
                    .Count(h => h.Type == PublicHoliday && h.Date >= startDate && h.Date <= endDate);


            #endregion New Calculation Done
        }
    }
}
