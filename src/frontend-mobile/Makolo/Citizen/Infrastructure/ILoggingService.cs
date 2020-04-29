using Microsoft.Extensions.Logging;
using System;

namespace Citizen.Infrastructure
{
    public interface ILoggingService : ILogger
    {
        void Debug(string message);

        void Warning(string message);

        void Error(Exception exception);
    }
}
