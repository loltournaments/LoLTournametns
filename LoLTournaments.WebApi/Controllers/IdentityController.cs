using System.Diagnostics.CodeAnalysis;
using LoLTournaments.Application.Exceptions;
using LoLTournaments.Application.Services;
using LoLTournaments.Shared.Models;
using LoLTournaments.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoLTournaments.WebApi.Controllers
{

    [Route("api/" + VersionInfo.APIVersion + "/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService identityService;
        private readonly IAccountInfoService accountInfoService;

        public IdentityController(
            IIdentityService identityService,
            IAccountInfoService accountInfoService)
        {
            this.identityService = identityService;
            this.accountInfoService = accountInfoService;
        }

        /// <summary>
        /// Creates new user if it doesnt exist.
        /// </summary>
        /// <response code="400">Wrong name</response>
        /// <response code="409">User with same name already exist</response>
        [HttpPost]
        [Route(nameof(Register))]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody, NotNull] Account model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                    throw new ClientException($"{nameof(model.UserName)} must be provided to perform sign-up");

                if (string.IsNullOrWhiteSpace(model.Password))
                    throw new ClientException($"{nameof(model.Password)} must be provided to perform sign-up");

                return Ok(await identityService.Register(model));
            }
            catch (ForbiddenException e)
            {
                return Forbid(e.Message);
            }
            catch (ClientException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Login with UserName and Password.
        /// </summary>
        [HttpPost]
        [Route(nameof(Login))]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody, NotNull] Account model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                    throw new ClientException($"{nameof(model.UserName)} must be provided to perform sign-in");

                if (string.IsNullOrWhiteSpace(model.Password))
                    throw new ClientException($"{nameof(model.Password)} must be provided to perform sign-in");

                return Ok(await identityService.Login(model));
            }
            catch (ForbiddenException e)
            {
                return Forbid(e.Message);
            }
            catch (ClientException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost(nameof(ResetPassword))]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody, NotNull] Account model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                    throw new ClientException($"{nameof(model.UserName)} must be provided to reset password");

                if (string.IsNullOrWhiteSpace(model.Password))
                    throw new ClientException($"{nameof(model.Password)} must be provided to reset password");
                
                return Ok(await identityService.ResetPassword(model));
            }
            catch (ClientException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost(nameof(GetAccountInfo))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAccountInfo([FromBody, NotNull] Account model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                    throw new ClientException($"{nameof(model.UserName)} must be provided to get account info.");

                return Ok(await accountInfoService.GetInfo(model.UserName));
            }
            catch (ClientException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Get current runtime app settings
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetConfig))]
        [AllowAnonymous]
        public Task<IActionResult> GetConfig()
        {
            try
            {
                return Task.FromResult<IActionResult>(Ok(identityService.GetConfig()));
            }
            catch (ClientException e)
            {
                return Task.FromResult<IActionResult>(BadRequest(e.Message));
            }
            catch (Exception e)
            {
                return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError, e.Message));
            }
        }

        /// <summary>
        /// Get current api time
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetApiTime))]
        [AllowAnonymous]
        public Task<IActionResult> GetApiTime()
        {
            try
            {
                return Task.FromResult<IActionResult>(Ok(identityService.GetApiTime()));
            }
            catch (ClientException e)
            {
                return Task.FromResult<IActionResult>(BadRequest(e.Message));
            }
            catch (Exception e)
            {
                return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError, e.Message));
            }
        }
    }

}