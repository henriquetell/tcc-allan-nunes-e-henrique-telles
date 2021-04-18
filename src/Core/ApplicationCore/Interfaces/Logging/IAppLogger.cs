using System;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces.Logging
{
    public interface IAppLogger
    {
        IAppLogger Info(string message,
            IDictionary<string, string> properties = null);
        IAppLogger Warning(string message,
            IDictionary<string, string> properties = null);

        IAppLogger Error(string message,
            IDictionary<string, string> properties = null);

        IAppLogger Exception(Exception ex, string message = null,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null);

        IAppLogger Event(ELoggingEvents eventId,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null);

    }

    public interface IAppLogger<TCategory> : IAppLogger
    {
    }
}
