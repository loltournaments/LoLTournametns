using CCG.Berserk.Application.Exceptions;
using LoLTournaments.Application.Abstractions;
using LoLTournaments.Application.Services;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Domain.Entities;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;
using LoLTournaments.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoLTournaments.WebApi.Controllers
{
	[Route("api/" + VersionInfo.APIVersion + "/[controller]")]
	[ApiController]
	[Authorize]
	// [ServiceFilter(typeof(ActivityFilter))] TODO remake
	public class IdentityController : SharedController
	{
		private readonly IAppSettings appSettings;
		private readonly IDbRepository dbRepository;
		private readonly IIdentityService identityService;

		public IdentityController(
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
		
		/// <summary>
		/// Login with UserName and Password.
		/// </summary>
        [HttpPost]
        [Route(nameof(Login))]
        [AllowAnonymous]
        public async Task<IActionResult> Login() // TODO [FromBody, NotNull] AuthLoginModel authModel
		{
			var user = await dbRepository.Get<UserEntity>().FirstOrDefaultAsync(); // TODO x => x.Email.ToLower() == authModel.Email.ToLower()
	        
            if (user != null && appSettings.IsMaintenanceMode && (user.Permission.HasFlag(Permissions.Developer) 
                                                                  || user.Permission.HasAnyFlags(Permissions.Manager)))
                return BadRequest($"We are updating game servers to provide you with the best gaming experience possible.");
  
            if (!IsValidVersion("", appSettings.Version, out var versionResult))
	            return versionResult;
            
            // if (string.IsNullOrWhiteSpace(authModel.Email))
            //     return BadRequest($"{nameof(authModel.Email)} must be provided to perform sign-up");
            //
            // if (string.IsNullOrWhiteSpace(authModel.Password))
            //     return BadRequest($"{nameof(authModel.Password)} must be provided to perform sign-up");
  
            try
            {
	            await identityService.Login(); // TODO result
	            return Ok();
            }
            catch (ServerException ex)
            {
	            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
			catch (ForbiddenException ex)
            {
	            return Forbid(ex.Message);
            }
            catch (ValidationException ex)
            {
	            return BadRequest(ex.Message);
            }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
        }
		
		/// <summary>
		/// Perform sign-in if the token is valid and not expired and updates access token.
		/// </summary>
		[HttpPost]
		[Route(nameof(Authenticate))]
		[AllowAnonymous]
		public async Task<IActionResult> Authenticate() // TODO [FromBody, NotNull] AuthTokenModel authModel
		{
            // if (string.IsNullOrWhiteSpace(authModel.Token))
            //     return BadRequest($"{nameof(authModel.Token)} must be provided to perform sign-in");
            
            if (!IsValidVersion("", appSettings.Version, out var versionResult))
	            return versionResult;
  
            try
            {
	            await identityService.Authenticate(); // TODO Result
	            return Ok();
            }
            catch (UnauthorizedHttpException e)
            {
	            return Unauthorized(e.Message);
            }
            catch (ServerException e)
            {
	            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (Exception e)
            {
	            return BadRequest(e.Message);
            }
		}
  
		[HttpPost(nameof(ResetPassword))]
		[AllowAnonymous]
		public async Task<IActionResult> ResetPassword() // TODO [FromBody, NotNull] ResetPasswordModel model
		{
			// if (string.IsNullOrWhiteSpace(model.Email))
			// 	return BadRequest($"{nameof(model.Email)} must be provided to reset password");
			//
			// if (string.IsNullOrWhiteSpace(model.EmailCode))
			// 	return BadRequest($"{nameof(model.EmailCode)} must be provided to reset password");
			//
			// if (string.IsNullOrWhiteSpace(model.Password))
			// 	return BadRequest($"{nameof(model.Password)} must be provided to reset password");
  
			try
			{
				await identityService.ResetPassword();
				return Ok();
			}
			catch (ServerException e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
			catch (ClientException e)
			{
				return BadRequest(e.Message);
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
			catch (ServerException e)
			{
				return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError, e.Message));
			}
			catch (ClientException e)
			{
				return Task.FromResult<IActionResult>(BadRequest(e.Message));
			}
		}
	}
}
