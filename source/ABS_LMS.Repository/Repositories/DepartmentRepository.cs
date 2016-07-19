using System.Runtime.Remoting.Contexts;
using ABS_LMS.Data;
using ABS_LMS.Repository.Interface;

namespace ABS_LMS.Repository.Repositories
{
    public class DepartmentRepository : Repository<Data.Department>, IDepartmentRepository
    {
        public DepartmentRepository(ABSLMSEntities context)
            : base(context)
        {
        }
        public ABSLMSEntities AbsContext => Context as ABSLMSEntities;
    }
}
