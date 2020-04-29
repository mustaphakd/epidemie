using Backend.DataTransferObjects;
using Refit;
using Refit.Insane.PowerPack.Caching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Citizen.Rest
{
    public interface IClientApi

    {

        [Post("/login")]
        Task<AuthModel> Login(UserLogin details, CancellationToken cancellationToken = default(CancellationToken));

        [Post("/security/register")]
        Task register(UserRegistration registrationDetails, CancellationToken cancellationToken = default(CancellationToken));


        // [RefitCache(1600)]


    }
}
