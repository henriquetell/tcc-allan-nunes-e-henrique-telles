using System;

namespace ApplicationCore.Respositories
{
    public interface IRepositoryTransaction : IDisposable
    {
        void Commit();
    }
}
