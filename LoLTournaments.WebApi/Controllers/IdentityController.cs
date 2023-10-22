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
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

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
            catch (Exception e)
            {
                return HandleException(e);
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
            catch (Exception e)
            {
                return HandleException(e);
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
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpGet(nameof(GetConfig))]
        [AllowAnonymous]
        public Task<IActionResult> GetConfig()
        {
            try
            {
                return Task.FromResult<IActionResult>(Ok(identityService.GetConfig()));
            }
            catch (Exception e)
            {
                return Task.FromResult(HandleException(e));
            }
        }

        [HttpGet(nameof(GetApiTime))]
        [AllowAnonymous]
        public Task<IActionResult> GetApiTime()
        {
            try
            {
                return Task.FromResult<IActionResult>(Ok(identityService.GetApiTime()));
            }
            catch (Exception e)
            {
                return Task.FromResult(HandleException(e));
            }
        }

        private IActionResult HandleException(Exception exception)
        {
            return exception switch
            {
                ForbiddenException => Forbid(exception.Message),
                ClientException => BadRequest(exception.Message),
                ValidationException => BadRequest(exception.Message),
                _ => BadRequest(StatusCode(StatusCodes.Status500InternalServerError, exception.Message)),
            };
        }
    }

}