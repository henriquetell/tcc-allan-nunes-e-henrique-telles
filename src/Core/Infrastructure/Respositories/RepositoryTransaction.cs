using ApplicationCore.Interfaces.Logging;
using ApplicationCore.Respositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Respositories
{
    internal class RepositoryTransaction : IRepositoryTransaction
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAppLogger<RepositoryTransaction> _appLogger;
        private IDbContextTransaction _transaction;

        public RepositoryTransaction(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _appLogger = _serviceProvider.GetService<IAppLogger<RepositoryTransaction>>();
        }

        internal void BeginTransaction()
        {
            var database = _serviceProvider.GetService<EfContext>().Database;
            if (database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                _transaction = database.BeginTransaction();
            _appLogger.Info($"Transação com {nameof(EfContext)} iniciado");
        }

        public void Commit()
        {
            _appLogger.Info($"Realizando commit da transação com {nameof(EfContext)}");

            try
            {
                _transaction?.Commit();
                _appLogger.Info($"Commit da transação com {nameof(EfContext)} foi realizado com sucesso");
            }
            catch (Exception ex)
            {
                _appLogger.Exception(ex, $"Commit da transação com {nameof(EfContext)} foi realizado com erro");
                throw;
            }
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
