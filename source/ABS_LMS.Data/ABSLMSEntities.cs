using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS_LMS.Data
{
    // ReSharper disable once InconsistentNaming
    public partial class ABSLMSEntities
    {
        public ABSLMSEntities(string connectionString)
            : base(connectionString)
        {
        }
    }
}
