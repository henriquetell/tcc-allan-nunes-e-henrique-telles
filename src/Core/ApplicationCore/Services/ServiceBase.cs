using ApplicationCore.Extenders;
using ApplicationCore.Interfaces.Logging;
using ApplicationCore.Respositories;
using Framework.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace ApplicationCore.Services
{
    public abstract class ServiceBase
    {
        private readonly IServiceProvider _serviceProvider;

        protected IAppLogger AppLogger => _serviceProvider.AppLogger(GetType());
        protected AppConfig AppConfig => GetService<IOptions<AppConfig>>()?.Value;
        protected ServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected TService GetService<TService>()
        {
            if (typeof(IRepository).IsAssignableFrom(typeof(TService)))
            {
                return (TService)_serviceProvider.GetService<RepositoryFactory>().Create(typeof(TService));
            }

            return _serviceProvider.GetService<TService>();

        }

        protected IRepositoryTransaction InitTransaction() => _serviceProvider.GetService<RepositoryFactory>().InitTransaction();
    }
}
