using Backend.Core.Infrastructure;
using Backend.Helpers;
using Backend.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Security
{
    public class SignInHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignInHandler(SignInManager<User> signInManager,  IHttpContextAccessor httpContextAccessor)
        {
            Check.NotNull(signInManager, nameof(signInManager));
            Check.NotNull(httpContextAccessor, nameof(httpContextAccessor));
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SignInResult> LoginAsync(User user, bool isPersistent, string scheme = null)
        {
            // perform complex logic before authentication user

            if (user == null)
            {
                throw new BackendException(Convert.ToInt32(HttpStatusCode.BadRequest), "Invalid credentials.");
            }

            var result = SignInResult.Success; // await _signInManager.CheckPasswordSignInAsync(user, user.Password, false);

            /**
             protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
	{
		ClaimsIdentity id = await base.GenerateClaimsAsync(user);
		if (base.UserManager.SupportsUserRole)
		{
			foreach (string item in await base.UserManager.GetRolesAsync(user))
			{
				id.AddClaim(new Claim(base.Options.ClaimsIdentity.RoleClaimType, item));
				if (RoleManager.SupportsRoleClaims)
				{
					TRole val = await RoleManager.FindByNameAsync(item);
					if (val != null)
					{
						ClaimsIdentity claimsIdentity = id;
						claimsIdentity.AddClaims(await RoleManager.GetClaimsAsync(val));
					}
				}
			}
		}
		return id;
	}
             */

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.String) },"makolo"));  ///await _signInManager.CreateUserPrincipalAsync(user);

            scheme = scheme ?? IdentityConstants.ApplicationScheme;

            await _httpContextAccessor.HttpContext.SignInAsync(
                scheme,
                userPrincipal,
                new AuthenticationProperties());

            return result;
        }

        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }
    }
}
