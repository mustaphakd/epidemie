using Backend.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Akavache;
using Citizen.Core;
using Microsoft.Extensions.Logging;
using Backend.Helpers;
using Citizen.Services.RequestSettings;
using Citizen.Infrastructure;
using Xamarin.Forms;

namespace Citizen.Services.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private ILoggingService logger_;
        public AuthenticationService()
        {
            var logger = DependencyService.Get<ILoggingService>();
            Check.NotNull(logger, nameof(logger));
            this.logger_ = logger;
        }
        public async Task<Operations.OperationResult<bool>> LoginAsync(UserLogin credentials)
        {
            Check.NotNull(credentials, nameof(credentials));
            AuthModel authModel = null;

            if(string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
            {
                return "error";
            }

            return true;

            try
            {
                //pull from cache
                authModel = await BlobCache.UserAccount.GetObject<AuthModel>(CacheKeys.User_Auth);
            }
            catch (KeyNotFoundException ex)
            {
                logger_.LogInformation($"AuthenticationService::LoginAsync() Failed to retrieve authmodel from cache.");
            }

            //ok return that
            if (authModel != null)
            {
                RegisterAuthTokenForRequests(authModel);
                return true;
            }

            // using polly
            //try to authenticate no more than 3 times
            //onFailure notify user via pop up
            var response = await SharedSettings.RestService.Execute<IAuthenticationEndpoint, AuthModel>(api => api.LoginAsync(credentials, default(System.Threading.CancellationToken)));

            if(response.IsSuccess == false)
            {
                return response.FormattedErrorMessages ?? "An error occured trying to log user in";
            }

            authModel = response.Results;
            await BlobCache.UserAccount.InsertObject<AuthModel>(CacheKeys.User_Auth, authModel);
            RegisterAuthTokenForRequests(authModel);

            return true;
        }

        private void RegisterAuthTokenForRequests(AuthModel authModel)
        {
            logger_.LogInformation($"AuthenticationService::RegisterAuthTokenForRequests() - Start Registering authTOken: ", authModel);

            var authenticatedHandler = new Func<Task<string>>(() =>
            {
                var localToken = authModel.Token;
                return Task.FromResult(localToken);
            });

            SharedSettings.RefitSettings.AuthorizationHeaderValueGetter = authenticatedHandler;

            //todo: add it also for graphQl bearer
        }

        public async Task<Operations.OperationResult<bool>> Logout()
        {
            // clearCache
            await BlobCache.UserAccount.InvalidateObject<AuthModel>(CacheKeys.User_Auth);
            //sent request to server
            //using polly 3 times max internal to powerpack
            var response = await SharedSettings.RestService.Execute<IAuthenticationEndpoint>(api => api.Logout(default(System.Threading.CancellationToken)));

            if (response.IsSuccess == false)
            {
                return response.FormattedErrorMessages ?? "An error occured trying to log user in";
            }

            return true;
        }

        public async Task<Operations.OperationResult<bool>> RegisterNewUserAsync(UserRegistration details)
        {
            //check cache for existing
            //currently update not enabled.
            //if exist return error message
            UserRegistration userDetails = null;
            Check.NotNull(details, nameof(details));


            if (string.IsNullOrEmpty(details.Email) || string.IsNullOrEmpty(details.Password))
            {
                return "Error";
            }

            return true;

            try
            {
                //pull from cache
                userDetails = await BlobCache.Secure.GetObject<UserRegistration>(CacheKeys.User_Profile);
            }
            catch (KeyNotFoundException ex)
            {
                logger_.LogInformation($"AuthenticationService::LoginAsync() Failed to retrieve authmodel from cache.");
            }

            //ok return that
            if (userDetails != null)
            {
                return "$We currently do not supporting updating your registration details";
            }

            //using polly max 5 times send request to server
            //if succeed, add to cache
            // else return errormessage
            var response = await SharedSettings.RestService.Execute<IAuthenticationEndpoint>(api => api.RegisterNewUserAsync(details, default(System.Threading.CancellationToken)));

            if (response.IsSuccess == false)
            {
                return response.FormattedErrorMessages ?? "An error occured trying to register user";
            }

            await BlobCache.Secure.InsertObject<UserRegistration>(CacheKeys.User_Profile, details);
            return true;

        }
    }
}
