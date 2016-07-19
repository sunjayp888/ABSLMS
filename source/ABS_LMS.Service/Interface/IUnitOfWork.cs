using System;

namespace ABS_LMS.Service.Interface
{
    public interface IUnitOfWork : IDisposable
    {
       int Complete();
    }
}
