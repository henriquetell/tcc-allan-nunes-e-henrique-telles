using ApplicationCore.Extenders;
using ApplicationCore.Interfaces.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Admin.Services
{
    public abstract class BaseServiceWeb
    {
        private readonly IServiceProvider _serviceProvider;

        protected IAppLogger AppLogger => _serviceProvider.AppLogger(GetType());

        protected BaseServiceWeb(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected TService GetService<TService>() => _serviceProvider.GetService<TService>();
    }
}
