using ApplicationCore.Interfaces.Logging;
using Framework.Extenders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CNA.BemMaisAgro.Infrastructure.Logging
{
    internal class BasicAppLogger<TCategory> : IAppLogger<TCategory>
    {
        private readonly ILogger<TCategory> _appLogger;

        public BasicAppLogger(ILoggerFactory loggerFactory)
        {
            _appLogger = loggerFactory.CreateLogger<TCategory>();
        }

        public IAppLogger Info(string message, IDictionary<string, string> properties = null)
        {
            _appLogger.LogInformation((message ?? "") + JoinPropertyMetricsToJson(properties));

            return this;
        }

        public IAppLogger Warning(string message, IDictionary<string, string> properties = null)
        {
            _appLogger.LogWarning((message ?? "") + JoinPropertyMetricsToJson(properties));

            return this;
        }

        public IAppLogger Error(string message, IDictionary<string, string> properties = null)
        {
            _appLogger.LogError((message ?? "") + JoinPropertyMetricsToJson(properties));

            return this;
        }

        public IAppLogger Exception(Exception ex, string message = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            _appLogger.LogError(ex, (message ?? "") + JoinPropertyMetricsToJson(properties, metrics));

            return this;
        }

        public IAppLogger Event(ELoggingEvents eventId, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            var evt = new EventId((int)eventId, eventId.GetDescription());

            _appLogger.LogTrace(evt, null);

            return this;
        }

        private string JoinPropertyMetricsToJson(IDictionary<string, string> properties, IDictionary<string, double> metrics = null)
        {
            if (properties == null && metrics == null) return null;

            var opt = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include
            };

            return JsonConvert.SerializeObject(new
            {
                Properties = properties,
                Metrics = metrics
            }, opt);
        }

    }
}
