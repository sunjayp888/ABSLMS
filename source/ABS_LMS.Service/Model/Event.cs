using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS_LMS.Service.Model
{
    public class Event
    {
        public int EventId { get; set; }
        public DateTime? DisplayStartDate { get; set; }
        public DateTime? DisplayEndDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? OrganiseDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
