using ApplicationCore.Interfaces.Logging;
using System;
using System.Diagnostics;

namespace ApplicationCore.Extenders
{
    public class LoggingWithTimer
    {
        private readonly IAppLogger _appLogger;
        private readonly Stopwatch _timer;
        private bool Stoped { get; set; } = false;

        internal LoggingWithTimer(IAppLogger appLogger)
        {
            _appLogger = appLogger;
            _timer = Stopwatch.StartNew();
        }

        public void Stop(Action<long, IAppLogger> action)
        {
            if (Stoped) throw new InvalidOperationException("O timer está pausado");

            _timer.Stop();
            Stoped = true;

            if (action != null)
                action.Invoke(_timer.ElapsedMilliseconds, _appLogger);
        }
    }
}
