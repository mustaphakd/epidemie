﻿
using Backend.Helpers;
using Backend.Services.Core;
using Backend.Services.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Security
{
    internal class AuthenticationService : Microsoft.AspNetCore.Authentication.AuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly DefaultUserAccessor _userAccessor;

        public AuthenticationService(IUserAccessor userAccessor, IAuthenticationSchemeProvider schemes, IAuthenticationHandlerProvider handlers, IClaimsTransformation transform, IOptions<AuthenticationOptions> options, ILogger<AuthenticationService> logger)
        : base(schemes, handlers, transform, options)
        {
            _logger = logger;

            var accessorType = userAccessor.GetType();

            if (!typeof(DefaultUserAccessor).IsAssignableFrom(accessorType))
            {
                throw new InvalidOperationException("Feed.Web.Services.AuthenticationService::ctr() -- Unsuported IUserAccessor implementation: " + accessorType);
            }

            _userAccessor = (DefaultUserAccessor)userAccessor;
        }

        /// <summary>
        /// This method is really only of use during login operation.  authentication middle ware will auto login user using the correct schemehandler registered before even entering userLand.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public override async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            var message = $"scheme: {scheme}. isUserAuthenticated: {context.User?.Identity?.IsAuthenticated}";
            Check.CallerLog<AuthenticationService>(_logger, LoggerExecutionPositions.Entrance, message);

            var feature = context.Features.Get<IAuthenticationFeature>();

            if (feature != null && IsOnPublicPath(feature.OriginalPath))
            {
                return AuthenticateResult.NoResult();
            }

            var handler = await this.Handlers.GetHandlerAsync(context, scheme);
            //var result =  await base.AuthenticateAsync(context, scheme);
            var result = await handler.AuthenticateAsync();

            if (result.Succeeded == false && result.Failure == null)
            {
                result = AuthenticateResult.Fail("User not authenticated");
            }


            return result;
        }

        static bool IsOnPublicPath(PathString pathString)
        {
            var publicPaths = new []{ "graphql", "index.html" };

            if (string.IsNullOrEmpty(pathString.Value)) { return true; }

            var root = pathString.Value
                .Trim('/')
                .Split('/')[0]
                .ToLowerInvariant();

            return publicPaths.Contains(root);

        }

        public override async Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            var message = $"scheme: {scheme}. isUserAuthenticated: {context.User?.Identity?.IsAuthenticated}";
            Check.CallerLog<AuthenticationService>(_logger, LoggerExecutionPositions.Entrance, message);

            if (context.Request.IsOnlyJsonOrAjaxRequest()) //Bearer scheme should only be allowed form mobile and front ends
            {
                if (scheme == JwtBearerDefaults.AuthenticationScheme)
                {
                    await base.ChallengeAsync(context, scheme, properties);
                }
                //else if ()
            }
            else if (scheme != JwtBearerDefaults.AuthenticationScheme)
            {
                await base.ChallengeAsync(context, scheme, properties);
            }


            message = $"scheme: {scheme}. isUserAuthenticated: {context.User?.Identity?.IsAuthenticated}";
            Check.CallerLog<AuthenticationService>(_logger, LoggerExecutionPositions.Exit, message);
        }

        public override async Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            Check.NotNull(principal, nameof(principal));
            Check.CallerLog<AuthenticationService>(_logger, LoggerExecutionPositions.Entrance, $"Scheme: [{scheme}], UserId: [{context.User}]");

            if (scheme != JwtBearerDefaults.AuthenticationScheme)
            {
                await base.SignInAsync(context, scheme, principal, properties);
            }

            context.User = principal;
            //_userAccessor.User = principal;

            if (scheme != JwtBearerDefaults.AuthenticationScheme && !context.Request.IsOnlyJsonOrAjaxRequest())
            {
                return;
            }

            // Get valid claims and pass them into JWT
            var claims = await this._userAccessor.GetUserClaims();

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: Constant.TokenSecurity.JwtAuthenticationAuthority,
                audience: Constant.TokenSecurity.JwtAuthenticationAudience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(58), //token should expire after 58 minutes :)
                signingCredentials: new SigningCredentials(Constant.TokenSecurity.jwtAuthenticationIssuerSigningKey, SecurityAlgorithms.HmacSha256));

            context.Items[Constant.AuthTokenKey] = new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
