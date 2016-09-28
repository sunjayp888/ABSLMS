using ABS_LMS.Service.Model;
using PagedList;

namespace ABS_LMS.Models
{
    public class HolidayViewModel
    {
        public IPagedList<Holiday> Holidays { get; set; }
    }
}