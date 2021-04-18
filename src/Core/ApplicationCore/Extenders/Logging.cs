using ApplicationCore.Interfaces.Logging;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Extenders
{
    public static class Logging
    {
        public static IAppLogger AppLogger<TCategory>(this IServiceProvider serviceProvider) =>
            serviceProvider.GetService<IAppLoggerFactory>().Create<TCategory>();

        public static IAppLogger AppLogger(this IServiceProvider serviceProvider, Type tCategory) =>
            serviceProvider.GetService<IAppLoggerFactory>().Create(tCategory);

        public static IAppLogger WarningIf(this IAppLogger logger, bool condition, string message,
            IDictionary<string, string> properties = null)
        {
            if (condition) logger.Warning(message, properties);
            return logger;
        }

        public static IAppLogger InfoIf(this IAppLogger logger, bool condition, string message,
            IDictionary<string, string> properties = null)
        {
            if (condition) logger.Info(message, properties);
            return logger;
        }

        public static IAppLogger ErrorIf(this IAppLogger logger, bool condition, string message,
            IDictionary<string, string> properties = null)
        {
            if (condition) logger.Error(message, properties);
            return logger;
        }

        public static IAppLogger ExceptionIf(this IAppLogger logger, bool condition, Exception ex, string message = null,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            if (condition) logger.Exception(ex, message, properties, metrics);
            return logger;
        }

        public static IAppLogger EventIf(this IAppLogger logger, bool condition, ELoggingEvents evt,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            if (condition) logger.Event(evt, properties, metrics);
            return logger;
        }

        public static LoggingWithTimer StartTimer(this IAppLogger logger) => new LoggingWithTimer(logger);


    }
}
