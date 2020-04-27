using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Services.Core
{
    public interface IUserAccessor
    {
        ClaimsPrincipal User { get; }

        Task<bool> IsUserInRole(string role);

        Task<IEnumerable<Claim>> GetUserClaims();

        string UserIdentier { get; }
    }
}
