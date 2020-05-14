using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Backend.DataTransferObjects;
using Citizen.Core;
using Refit;
using Refit.Insane.PowerPack.Retry;

[assembly: RefitRetryAttribute(3)]
namespace Citizen.Services
{
    public interface IAuthenticationEndpoint
    {
        [RefitRetry]
        [Post(RestEndpoints.Login)]
        Task<AuthModel> LoginAsync(UserLogin credentials, CancellationToken cancellationToken = default(CancellationToken));

        [RefitRetry]
        [Post(RestEndpoints.Registation)]
        Task RegisterNewUserAsync(UserRegistration details, CancellationToken cancellationToken = default(CancellationToken));

        [Post(RestEndpoints.Logout)] //<System.Net.HttpStatusCode>
        Task Logout(CancellationToken cancellationToken = default(CancellationToken));
    }
}
