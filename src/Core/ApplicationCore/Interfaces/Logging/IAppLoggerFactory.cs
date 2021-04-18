using System;

namespace ApplicationCore.Interfaces.Logging
{
    public interface IAppLoggerFactory
    {
        IAppLogger Create<TCategory>();

        IAppLogger Create(Type category);
    }
}
