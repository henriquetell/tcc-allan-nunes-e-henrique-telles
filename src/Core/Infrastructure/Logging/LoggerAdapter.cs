using ApplicationCore.Interfaces.Logging;
using System;
using System.Collections.Generic;

namespace Infrastructure.Logging
{
    internal class LoggerAdapter : IAppLogger
    {
        private readonly IEnumerable<IAppLogger> _appLogger;

        public LoggerAdapter(IEnumerable<IAppLogger> appLogger)
        {
            _appLogger = appLogger;
        }

        public IAppLogger Info(string message, IDictionary<string, string> properties = null)
        {
            foreach (var item in _appLogger)
                item.Info(message, properties);

            return this;
        }

        public IAppLogger Warning(string message, IDictionary<string, string> properties = null)
        {
            foreach (var item in _appLogger)
                item.Warning(message, properties);

            return this;
        }

        public IAppLogger Error(string message, IDictionary<string, string> properties = null)
        {
            foreach (var item in _appLogger)
                item.Error(message, properties);

            return this;
        }

        public IAppLogger Exception(Exception ex, string message = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            foreach (var item in _appLogger)
                item.Exception(ex, message, properties, metrics);

            return this;
        }

        public IAppLogger Event(ELoggingEvents eventId, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            foreach (var item in _appLogger)
                item.Event(eventId, properties, metrics);

            return this;
        }
    }
}
