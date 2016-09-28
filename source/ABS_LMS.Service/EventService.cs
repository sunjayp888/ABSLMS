using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS_LMS.Data;
using ABS_LMS.Service.Interface;
using Event = ABS_LMS.Service.Model.Event;

namespace ABS_LMS.Service
{
    public class EventService : IEventService
    {
        private readonly UnitOfWork _unitOfWork;
        public EventService()
        {
            _unitOfWork = new UnitOfWork(new ABSLMSEntities(ConfigHelper.ConnectionString));
        }
        public void DeleteEvent(int id)
        {
            _unitOfWork.Event.Remove(_unitOfWork.Event.Get(id));
            _unitOfWork.Complete();
        }

        public List<Event> GetEvents()
        {
            var events = _unitOfWork.Event.GetAll();
            return events.Select(e => new Event
            {
                EventId = e.EventId,
                Description = e.Description,
                DisplayStartDate = e.StartDate,
                DisplayEndDate = e.EndDate,
                OrganiseDate = e.OrganiseDate,
                CreatedBy = e.CreatedBy,
                CreatedDate = e.CreatedDate,
                Title = e.Title,
                EventImage = e.EventImage
            }).OrderByDescending(e=>e.CreatedBy).ToList();
        }

        public void UpdateEvent(int id, Event events, string username)
        {
            var eventdetails = _unitOfWork.Event.Get(id);
            eventdetails.StartDate = events.DisplayStartDate;
            eventdetails.EndDate = events.DisplayEndDate;
            eventdetails.CreatedBy = username;
            eventdetails.Description = events.Description;
            eventdetails.OrganiseDate = events.OrganiseDate;
            eventdetails.Title = events.Title;
            eventdetails.EventImage = events.EventImage;
            _unitOfWork.Complete();
        }

        public void AddEvent(Event events, string username)
        {
            _unitOfWork.Event.Add(new Data.Event
            {
                CreatedBy = username,
                CreatedDate = DateTime.Now,
                EndDate = events.DisplayEndDate,
                Description = events.Description,
                OrganiseDate = events.OrganiseDate,
                StartDate = events.DisplayStartDate,
                Title = events.Title,
                EventImage = events.EventImage
                
            });
            _unitOfWork.Complete();
        }
    }
}
