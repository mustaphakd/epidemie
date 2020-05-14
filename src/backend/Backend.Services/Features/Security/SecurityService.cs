using Backend.Core.Infrastructure;
using Backend.DataTransferObjects;
using Backend.Helpers;
using Backend.Model;
using Backend.Repository;
using Backend.Services.Core;
using Backend.Services.Features.Common;
using Backend.Services.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Features.Security
{
    public class SecurityService : BaseService<SecurityService>
    {
        private SecurityRepository securityRepository_;
        private SignInHandler signInHandler_;

        public SecurityService(SecurityRepository securityRepository, SignInHandler signInHandler, IHttpContextAccessor httpContextAccessor, ILogger<SecurityService> logger)
            : base(httpContextAccessor, logger)
        {
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Entrance, $"securityRepository model: {securityRepository}");
            Check.NotNull(securityRepository, nameof(securityRepository));
            Check.NotNull(signInHandler, nameof(signInHandler));
            Check.NotNull(httpContextAccessor, nameof(httpContextAccessor));

            securityRepository_ = securityRepository;
            signInHandler_ = signInHandler;
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Exit);
        }

        internal async Task RegisterUser(UserRegistration model)
        {
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Entrance, $"registration model: {model}");
            Check.NotNull(model, nameof(model));
            //validate model first
             securityRepository_.RegisterUserAsync(
                new User { Email = model.Email, Password = model.Password },
                new Profile
                {
                    Birth = model.DateOfBirth,
                    FirstName = model.FirstName,
                    Gender = model.Gender,
                    LastName = model.LastName,
                    MaritalStatus = model.MaritalStatus,
                    Occupation = model.Occupation
                });
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Exit, $"registration Completed");
            await Task.CompletedTask;
        }

        internal async Task<AuthModel> LoginUser(UserLogin model)
        {
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Entrance, $"LoginUser model: {model}");
            Check.NotNull(model, nameof(model));
            //validate model first
            var user = new User { Email = model.Email, Password = model.Password };
            var userhasBeenValidated = securityRepository_.ValidateUser(user);

            if (userhasBeenValidated == false)
            {
                throw new BackendException(Convert.ToInt32(HttpStatusCode.BadRequest), "Please validate your account.");
            }

            await signInHandler_.LoginAsync(user, true, JwtBearerDefaults.AuthenticationScheme);
            var authoToken = HttpContextAccessor.HttpContext.Items[Constant.AuthTokenKey];

            //prevent use of session storage
            ClearSession();

            Check.NotNull(authoToken, nameof(authoToken), $"authToken could not be generated.");
            var authModel = new AuthModel(Convert.ToString(authoToken));
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Exit, $"LoginUser Completed - authModel: {authModel}");
            return authModel;
        }

        internal async Task Logout()
        {
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Entrance, $"Logout ");
            await signInHandler_.Logout();
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Exit, $"Logout Completed");
        }

        internal async Task validateAccount(string model)
        {
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Entrance, $"model: {model}");
            throw new NotImplementedException();
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Exit, $" Completed");
        }

        private void ClearSession()
        {
            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Entrance, $"ClearSession ");
            var context = HttpContextAccessor.HttpContext;

            foreach (var cookie in context.Request.Cookies.Keys)
            {
                Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Entrance, $"Deleting cookie with key: {cookie} ");
                context.Response.Cookies.Delete(cookie);
            }

            Check.CallerLog<SecurityService>(Logger, LoggerExecutionPositions.Exit, $"ClearSession Completed");
        }
    }
}
