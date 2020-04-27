using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services.DependencyInjection
{
    public class SecurityBuildersAccessor
    {
        public SecurityBuildersAccessor(IdentityBuilder identityBuilder, AuthenticationBuilder authenticationBuilder)
        {
            this.identityBuilder = identityBuilder;
            this.authenticationBuilder = authenticationBuilder;
        }

        public IdentityBuilder identityBuilder { get; private set; }
        public AuthenticationBuilder authenticationBuilder { get; private set; }
    }
}
