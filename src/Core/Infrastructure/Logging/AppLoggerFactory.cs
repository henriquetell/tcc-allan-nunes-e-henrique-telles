using ApplicationCore.Interfaces.Logging;
using Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CNA.BemMaisAgro.Infrastructure.Logging
{
    public class AppLoggerFactory : IAppLoggerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AppLoggerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAppLogger Create<TCategory>()
        {
            var adapter = new LoggerAdapter(_serviceProvider.GetServices<IAppLogger<TCategory>>());
            return adapter;
        }

        public IAppLogger Create(Type category)
        {
            var adapter = new LoggerAdapter(_serviceProvider.GetServices(typeof(IAppLogger<>).MakeGenericType(category)).Cast<IAppLogger>());
            return adapter;
        }
    }
}
