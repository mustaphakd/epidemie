using Backend.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.DataTransferObjects
{
    public class AuthModel
    {
        public AuthModel(string token)
        {
            Check.NotNull(token, nameof(token));
            Token = token;
        }

        public string Token { get; }
    }
}
