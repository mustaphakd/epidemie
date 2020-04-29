using System;
using System.Collections.Generic;
using System.Text;

namespace Citizen.Infrastructure
{
    public class LoggerFactory : Microsoft.Extensions.Logging.LoggerFactory
    {
        public LoggerFactory()
        {
            this.AddProvider(new LogProvider());
        }
    }
}
