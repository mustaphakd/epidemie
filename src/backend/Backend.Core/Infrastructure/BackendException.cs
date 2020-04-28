using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Infrastructure
{
    public sealed class BackendException : Exception
    {
        public BackendException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
        public int StatusCode { get; }

        public override string ToString()
        {
            return $"Status: {StatusCode}, Message: {Message}";
        }
    }
}
