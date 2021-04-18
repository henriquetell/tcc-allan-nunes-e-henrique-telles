using ApplicationCore.Extenders;
using ApplicationCore.Interfaces.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Infrastructure.CloudServices
{
    public abstract class CloudServiceBase
    {
        private readonly IServiceProvider _serviceProvider;

        protected IAppLogger AppLogger => _serviceProvider.AppLogger(GetType());

        protected CloudServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected TService GetService<TService>() => _serviceProvider.GetService<TService>();
    }
}
