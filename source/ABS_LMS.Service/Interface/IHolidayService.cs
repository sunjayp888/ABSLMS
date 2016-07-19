using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS_LMS.Service.Interface
{
    public interface IHolidayService
    {
        int GetActualLeaveDaysCount(DateTime startDate, DateTime endDate);
    }
}
