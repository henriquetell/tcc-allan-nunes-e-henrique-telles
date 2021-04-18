using ApplicationCore.Respositories;
using System;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class DefaultCudOperationFactory : RepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultCudOperationFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override IRepository Create(Type iRepositoryType)
        {
            var repositoryType = GetType().Assembly.GetTypes()
                .FirstOrDefault(t => typeof(RespositoryBase).IsAssignableFrom(t) &&
                                     iRepositoryType.IsAssignableFrom(t));

            return (IRepository)_serviceProvider.GetService(repositoryType);
        }

        protected override IRepositoryTransaction InitTransaction()
        {
            var ts = new RepositoryTransaction(_serviceProvider);
            ts.BeginTransaction();

            return ts;
        }
    }
}
