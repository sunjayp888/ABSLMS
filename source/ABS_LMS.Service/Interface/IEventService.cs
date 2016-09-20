using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS_LMS.Service.Model;

namespace ABS_LMS.Service.Interface
{
    public interface IEventService
    {
        List<Event> GetEvents();
        void UpdateEvent(int id, Event events, string username);
        void DeleteEvent(int id);
        void AddEvent(Event events,string username);
    }
}
