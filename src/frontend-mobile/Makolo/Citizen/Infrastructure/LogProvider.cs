using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citizen.Infrastructure
{
    public sealed class LogProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            //#if DEBUG
            //return new DebugLoggingService(categoryName);
            //#else
            return new FileLoggingService(categoryName);
//#endif
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
