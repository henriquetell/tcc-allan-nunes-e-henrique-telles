using ApplicationCore.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;

namespace NpsFunctions.Triggers
{
    public class BaseTrigger
    {
        private readonly ApplicationCoreConfig _applicationCoreConfig;
        private readonly ILogger _logger;
        public BaseTrigger(ILogger logger, IServiceProvider serviceProvider)
        {
            _applicationCoreConfig = (ApplicationCoreConfig)serviceProvider.GetService(typeof(ApplicationCoreConfig));
            _logger = logger;
        }

        protected bool ValidateAuthorization(HttpRequest reg)
        {
            try
            {
                return reg.Headers.TryGetValue("x-access-token", out StringValues accessToken) &&
                    _applicationCoreConfig.AccessToken == new Guid(accessToken.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro");
            }
            return false;
        }
    }
}
