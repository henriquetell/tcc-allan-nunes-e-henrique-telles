using System;

namespace ApplicationCore.Respositories
{
    public abstract class RepositoryFactory
    {
        protected internal abstract IRepository Create(Type repositoryType);

        protected internal abstract IRepositoryTransaction InitTransaction();
    }
}
