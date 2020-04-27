using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DataTransferObjects;
using Backend.Model;
using Backend.Services.Features.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;

namespace Backend.Services.Apis
{
    [Produces("application/json")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly ILogger<AuthenticationController> _logger;
        private readonly SecurityService securityService_;

        public AuthenticationController(SecurityService securityService , ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            securityService_ = securityService;
        }

        [HttpPost]
        [Route("security/register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<StatusCodeResult> RegisterUser([FromBody]UserRegistration model)
        {
            //System.Diagnostics.Debugger.Launch();
            //System.Diagnostics.Debugger.Break();
            _logger.LogDebug($"{nameof(RegisterUser)}() - Start - Model : {model}", model);
            await securityService_.RegisterUser(model);
            _logger.LogDebug($"{nameof(RegisterUser)}() - End ");
            return new OkResult();
        }

        [HttpPost]
        [Route("security/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ObjectResult> Login([FromBody]UserLogin model)
        {
            _logger.LogDebug($"{nameof(Login)}() - Start - Model : {model}", model);
            var authModel = await securityService_.LoginUser(model);
            _logger.LogDebug($"{nameof(Login)}() - End ");
            return new OkObjectResult(authModel);
        }


        [HttpPost]
        [Route("security/logout")]
        public async Task<StatusCodeResult> Logout()
        {
            _logger.LogDebug($"{nameof(Logout)}() - Start - ");
            await securityService_.Logout();
            _logger.LogDebug($"{nameof(Logout)}() - End ");
            return new OkResult();
        }



        [HttpGet]
        [Route("security/validateEmail")]
        public async Task<StatusCodeResult> ValidateAccount([FromQuery]string email)
        {
            _logger.LogDebug($"{nameof(ValidateAccount)}() - Start - Model : {email}", email);
            await securityService_.validateAccount(email);
            _logger.LogDebug($"{nameof(ValidateAccount)}() - End ");
            return new OkResult();
        }


    }
}
