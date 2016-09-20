using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS_LMS.Data;
using ABS_LMS.Repository.Interface;

namespace ABS_LMS.Repository.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(ABSLMSEntities context)
            : base(context)
        {
        }
        public ABSLMSEntities AbsContext => Context as ABSLMSEntities;

    }
}
