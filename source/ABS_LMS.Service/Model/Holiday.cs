using System;
using System.ComponentModel.DataAnnotations;

namespace ABS_LMS.Service.Model
{
    public class Holiday
    {
        public int HolidayId { get; set; }

        [Required(ErrorMessage = "Please Enter Holiday Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Holiday Date")]
        public DateTime? Date { get; set; }

        public string Day { get; set; }
        public string Type { get; set; }

        [Required(ErrorMessage = "Please Enter Description")]
        public string Description { get; set; }
    }
}
