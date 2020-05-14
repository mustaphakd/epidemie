using Backend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Services.Features.Common
{
    public abstract class BaseService<T> where T: BaseService<T>
    {
        public BaseService(IHttpContextAccessor httpContextAccessor, ILogger<T> logger)
        {
            Check.CallerLog<T>(logger, LoggerExecutionPositions.Entrance, $"-");
            Check.NotNull(httpContextAccessor, nameof(httpContextAccessor));
            Check.NotNull(logger, nameof(logger));
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
            Check.CallerLog<T>(Logger, LoggerExecutionPositions.Exit);
        }

        public ILogger<T> Logger { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
    }
}
