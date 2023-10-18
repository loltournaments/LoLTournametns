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
    public class LobbyController : ControllerBase
    {
        private readonly ILobbyService lobbyService;

        public LobbyController(ILobbyService lobbyService)
        {
            this.lobbyService = lobbyService;
        }

        /// <summary>
        /// Creates new user if it doesnt exist.
        /// </summary>
        /// <response code="400">Wrong name</response>
        /// <response code="409">User with same name already exist</response>
        [HttpPost]
        [Route(nameof(GetData))]
        [AllowAnonymous]
        public async Task<IActionResult> GetData([FromBody, NotNull] string request)
        {
            try
            {
                return Ok(await lobbyService.GetData(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}