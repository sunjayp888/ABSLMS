using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABS_LMS.Service.Model;
using PagedList;

namespace ABS_LMS.Models
{
    public class EventViewModel
    {
        public IPagedList<Event> Events { get; set; }
    }
}