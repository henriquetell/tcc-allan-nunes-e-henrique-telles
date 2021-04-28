using ApplicationCore.Respositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Respositories
{
    internal class RepositoryTransaction : IRepositoryTransaction
    {
        private readonly IServiceProvider _serviceProvider;
        private IDbContextTransaction _transaction;

        public RepositoryTransaction(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        internal void BeginTransaction()
        {
            var database = _serviceProvider.GetService<EfContext>().Database;
            if (database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                _transaction = database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;

                GC.SuppressFinalize(this);
            }
        }
    }
}
