using ABS_LMS.Service.Model;
using PagedList;

namespace ABS_LMS.Models
{
    public class EmployeeIndexViewModel
    {
        public IPagedList<Employee> EmployeeDetail { get; set; }
    }
}