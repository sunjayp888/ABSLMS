using System;
using System.Collections.Generic;
using System.Linq;
using ABS_LMS.Data;
using ABS_LMS.Service.Interface;

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
        }

        public List<Model.Holiday> GetAllPublicHolidaysList()
        {
            var holiday = _unitOfWork.Holiday.GetAll();
            return holiday.Where(h=>h.Type == "H").Select(h => new Model.Holiday
            {
                HolidayId = h.HolidayId,
                Date = h.Date,
                Day = h.Day,
                Description = h.Description,
                Type = h.Type
            }).ToList();
        }

        public void AddHoliday(Model.Holiday holiday)
        {
            _unitOfWork.Holiday.Add(new Holiday
            {
                Date = holiday.Date,
                Day = holiday.Date?.ToString("dddd"),
                Type = PublicHoliday,
                Description = holiday.Description
            });
            _unitOfWork.Complete();
        }

        public void DeleteHoliday(int holidayId)
        {
            _unitOfWork.Holiday.Remove(_unitOfWork.Holiday.Get(holidayId));
            _unitOfWork.Complete();
        }
    }
}
