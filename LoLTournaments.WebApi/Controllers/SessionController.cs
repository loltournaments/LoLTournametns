using CCG.Berserk.Application.Exceptions;
using LoLTournaments.Application.Abstractions;
using LoLTournaments.Application.Services;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoLTournaments.WebApi.Controllers
{

    [Route("api/" + VersionInfo.APIVersion + "/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionController : SharedController
    {
        private readonly IAppSettings appSettings;
        private readonly IDbRepository dbRepository;
        private readonly IIdentityService identityService;

        public SessionController(
            IAppSettings appSettings,
            IDbRepository dbRepository,
            IIdentityService identityService)
        {
            this.appSettings = appSettings;
            this.dbRepository = dbRepository;
            this.identityService = identityService;
        }
        
        /// <summary>
        /// Creates new user if it doesnt exist.
        /// </summary>
        /// <response code="400">Wrong name</response>
        /// <response code="409">User with same name already exist</response>
        [HttpPost]
        [Route(nameof(Register))]
        [AllowAnonymous]
        public async Task<IActionResult> Register()// [FromBody, NotNull] TODO
        {
            // if (string.IsNullOrWhiteSpace(authModel.UserName))
            // 	return BadRequest($"{nameof(authModel.UserName)} must be provided to perform sign-up");
            //
            // if (string.IsNullOrWhiteSpace(authModel.Email))
            // 	return BadRequest($"{nameof(authModel.Email)} must be provided to perform sign-up");
			
            if (!IsValidVersion("", appSettings.Version, out var versionResult)) // TODO version
                return versionResult;
  
            try
            {
                await identityService.Register();
                return Ok();
            }
            catch (ServerException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (ClientException e)
            {
                return Conflict(e.Message);
            }
            catch (ConflictException e)
            {
                return Conflict(e.Message);
            }
        }
    }

}