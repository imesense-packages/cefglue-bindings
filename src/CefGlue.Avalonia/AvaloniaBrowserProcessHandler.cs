using System;
using Avalonia.Threading;
using Xilium.CefGlue.Common.Handlers;

namespace Xilium.CefGlue.Avalonia
{
    internal class AvaloniaBrowserProcessHandler : BrowserProcessHandler
    {
        private DispatcherTimer _timer;
        private object _schedule = new object();

        protected override void OnScheduleMessagePumpWork(long delayMs)
        {
            lock (_schedule)
            {
                if (_timer != null)
                {
                    _timer.Stop();
                }

                if (delayMs <= 0)
                {
                    delayMs = 1;
                }

                _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(delayMs), DispatcherPriority.Background, (_, _) =>
                {
                    CefRuntime.DoMessageLoopWork();
                });
                _timer.Start();
            }
        }
    }
}
