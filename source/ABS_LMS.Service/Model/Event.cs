using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
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
        //[UIHint("tinymce_jquery_full"), AllowHtml]
        [AllowHtml]
        public string Description { get; set; }
        public DateTime? OrganiseDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Event Image")]
        public byte[] EventImage { get; set; }
    }
}
