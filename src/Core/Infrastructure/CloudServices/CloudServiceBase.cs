using Microsoft.Extensions.DependencyInjection;
using System;


namespace Infrastructure.CloudServices
{
    public abstract class CloudServiceBase
    {
        private readonly IServiceProvider _serviceProvider;

        protected CloudServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected TService GetService<TService>() => _serviceProvider.GetService<TService>();
    }
}
