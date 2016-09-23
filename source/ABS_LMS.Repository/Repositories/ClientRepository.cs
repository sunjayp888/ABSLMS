using ABS_LMS.Data;
using ABS_LMS.Repository.Interface;

namespace ABS_LMS.Repository.Repositories
{
    public class ClientRepository : Repository<Client> , IClientRepository
    {
        public ClientRepository(ABSLMSEntities context) :base(context)
        {}
        public ABSLMSEntities AbsContext => Context as ABSLMSEntities;
    }
}
