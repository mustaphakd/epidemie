using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citizen.Infrastructure
{
    public static class DynamicLoggerFactoryExtensions
    {
        public static ILoggingBuilder AddDebug(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, LogProvider>());
            return builder;
        }
    }
}
