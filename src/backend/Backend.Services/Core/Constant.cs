using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Services.Core
{
    public static class Constant
    {
        public const string AuthTokenKey = "authToken";
        internal static class TokenSecurity
        {
            public const string JwtAuthenticationAuthority = "http://makolo-xxx.com";
            public const string JwtAuthenticationAudience = "makolo-insights-resources";
            public const string JwtAuthenticationSubject = "makolo-insights-principals";
            private static readonly string secretKey = "iafe72r&Y)(*&Y@yr48at_secrfaw33tey!12353";
            public static readonly SymmetricSecurityKey jwtAuthenticationIssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(secretKey)
                );
        }
    }
}
