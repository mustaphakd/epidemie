using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Services.Features.Common
{
    public abstract class BaseService<T> where T: BaseService<T>
    {
        public BaseService(ILogger<T> logger)
        {
            Logger = logger;
        }

        public ILogger<T> Logger { get; private set; }
    }
}
