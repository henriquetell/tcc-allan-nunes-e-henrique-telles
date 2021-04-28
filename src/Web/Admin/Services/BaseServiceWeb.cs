using Microsoft.Extensions.DependencyInjection;
using System;

namespace Admin.Services
{
    public abstract class BaseServiceWeb
    {
        private readonly IServiceProvider _serviceProvider;

        protected BaseServiceWeb(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected TService GetService<TService>() => _serviceProvider.GetService<TService>();
    }
}
