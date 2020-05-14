using Backend.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Citizen.Services
{
    public interface IAuthenticationService
    {
        Task<Operations.OperationResult<bool>> LoginAsync(UserLogin credentials);

        Task<Operations.OperationResult<bool>> RegisterNewUserAsync(UserRegistration details);

        Task<Operations.OperationResult<bool>> Logout();
    }
}
