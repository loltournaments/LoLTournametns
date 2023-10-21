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

		public IdentityController(IIdentityService identityService)
		{
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
		public async Task<IActionResult> Register([FromBody, NotNull] UserDto model)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(model.UserName))
					return BadRequest($"{nameof(model.UserName)} must be provided to perform sign-up");
  	
				if (string.IsNullOrWhiteSpace(model.Password))
					return BadRequest($"{nameof(model.Password)} must be provided to perform sign-up");
				
				return Ok(await identityService.Register(model));
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
			catch (ForbiddenException ex)
			{
				return Forbid(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		/// <summary>
		/// Login with UserName and Password.
		/// </summary>
        [HttpPost]
        [Route(nameof(Login))]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody, NotNull] UserDto model)
		{
			try
            {
	            
	            if (string.IsNullOrWhiteSpace(model.UserName))
		            return BadRequest($"{nameof(model.UserName)} must be provided to perform sign-in");
            
	            if (string.IsNullOrWhiteSpace(model.Password))
		            return BadRequest($"{nameof(model.Password)} must be provided to perform sign-in");
	            
	            return Ok(await identityService.Login(model));
            }
            catch (ServerException ex)
            {
	            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
			catch (ForbiddenException ex)
            {
	            return Forbid(ex.Message);
            }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
        }

		[HttpPost(nameof(ResetPassword))]
		[AllowAnonymous]
		public async Task<IActionResult> ResetPassword([FromBody, NotNull] UserDto model)
		{
			if (string.IsNullOrWhiteSpace(model.UserName))
				return BadRequest($"{nameof(model.UserName)} must be provided to reset password");

			if (string.IsNullOrWhiteSpace(model.Password))
				return BadRequest($"{nameof(model.Password)} must be provided to reset password");
  
			try
			{
				return Ok(await identityService.ResetPassword(model));
			}
			catch (ServerException e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
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
			catch (Exception e)
			{
				return Task.FromResult<IActionResult>(BadRequest(e.Message));
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
			catch (ServerException e)
			{
				return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError, e.Message));
			}
			catch (Exception e)
			{
				return Task.FromResult<IActionResult>(BadRequest(e.Message));
			}
		}
	}

}
