using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABS_LMS.Models;
using ABS_LMS.Service.Interface;
using PagedList;
using ABS_LMS.Service.Model;
using ABS_LMS.Models.Security;

namespace ABS_LMS.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: Event
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int pagenumber = 1, int pagesize = 10)
        {
            var leaveDetails = _eventService.GetEvents();
            var model = new EventViewModel
            {
                Events = leaveDetails.ToPagedList(pagenumber, pagesize)
            };
            return View(model);
        }
        [Authorize(Roles = "Admin, Hr")]
        public ActionResult Create()
        {
         
            return  View();
        }

        [HttpPost]
        public ActionResult Create(Event model)
        {
           
                HttpPostedFileBase file = Request.Files["userimage"];
          
                var getbyte = ConvertToBytes(file);
                model.EventImage = getbyte;
                _eventService.AddEvent(model, HttpContext.User.Identity.Name);
        
               return RedirectToAction("Index");
            
        }
        [Authorize(Roles = "Admin, Hr")]
        public ActionResult Edit(int id)
        {
            var eventdetails = _eventService.GetEvents().FirstOrDefault(e => e.EventId == id);
            var model = new EventViewModel
            {
             EventDetail = eventdetails
            };
            TempData["EventImage"] = model.EventDetail.EventImage;
          
            return View(model);

        }

        [HttpPost]
        public ActionResult Edit(int id,EventViewModel model)
        {
            HttpPostedFileBase file = Request.Files["userimage"];
            var getbyte = ConvertToBytes(file);
            if (getbyte != null && getbyte.Length > 0)
            {
                model.EventDetail.EventImage = getbyte;
            }
            else
            {
                model.EventDetail.EventImage = TempData["EventImage"] as byte[];
            }
            _eventService.UpdateEvent(model.EventDetail.EventId, model.EventDetail, HttpContext.User.Identity.Name);
           
            return RedirectToAction("Index");

        }
        [Authorize(Roles = "Admin, Hr")]
        public ActionResult Delete(int id)
        {
            var result = 1;
            _eventService.DeleteEvent(id);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}