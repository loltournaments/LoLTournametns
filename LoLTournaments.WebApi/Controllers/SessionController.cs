using System.Diagnostics.CodeAnalysis;
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
    public class SessionController : ControllerBaseExtended
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        [HttpGet]
        [Route(nameof(GetSessions))]
        [AllowAnonymous]
        public async Task<IActionResult> GetSessions()
        {
            try
            {
                return Ok(await sessionService.GetSessions());
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(GetSession))]
        [AllowAnonymous]
        public async Task<IActionResult> GetSession([FromBody, NotNull] RequestSession model)
        {
            try
            {
                return Ok(await sessionService.GetSession(model));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(GetStage))]
        [AllowAnonymous]
        public async Task<IActionResult> GetStage([FromBody, NotNull] RequestStage model)
        {
            try
            {
                return Ok(await sessionService.GetStage(model));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(GetGroup))]
        [AllowAnonymous]
        public async Task<IActionResult> GetGroup([FromBody, NotNull] RequestGroup model)
        {
            try
            {
                return Ok(await sessionService.GetGroup(model));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(GetGame))]
        [AllowAnonymous]
        public async Task<IActionResult> GetGame([FromBody, NotNull] RequestGame model)
        {
            try
            {
                return Ok(await sessionService.GetGame(model));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(GetSessionData))]
        [AllowAnonymous]
        public async Task<IActionResult> GetSessionData([FromBody, NotNull] RequestSessionData model)
        {
            try
            {
                return Ok(await sessionService.GetSessionData(model));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(GetStageData))]
        [AllowAnonymous]
        public async Task<IActionResult> GetStageData([FromBody, NotNull] RequestStageData model)
        {
            try
            {
                return Ok(await sessionService.GetStageData(model));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(GetGroupData))]
        [AllowAnonymous]
        public async Task<IActionResult> GetGroupData([FromBody, NotNull] RequestGroupData model)
        {
            try
            {
                return Ok(await sessionService.GetGroupData(model));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(GetGameData))]
        [AllowAnonymous]
        public async Task<IActionResult> GetGameData([FromBody, NotNull] RequestGameData model)
        {
            try
            {
                return Ok(await sessionService.GetGameData(model));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(PostSessionData))]
        [AllowAnonymous]
        public async Task<IActionResult> PostSessionData([FromBody, NotNull] ReceiveSessionData model)
        {
            try
            {
                await sessionService.SetSessionData(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(PostStageData))]
        [AllowAnonymous]
        public async Task<IActionResult> PostStageData([FromBody, NotNull] ReceiveStageData model)
        {
            try
            {
                await sessionService.SetStageData(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(PostGroupData))]
        [AllowAnonymous]
        public async Task<IActionResult> PostGroupData([FromBody, NotNull] ReceiveGroupData model)
        {
            try
            {
                await sessionService.SetGroupData(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(PostGameData))]
        [AllowAnonymous]
        public async Task<IActionResult> PostGameData([FromBody, NotNull] ReceiveGameData model)
        {
            try
            {
                await sessionService.SetGameData(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(UpdateSession))]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateSession([FromBody, NotNull] ReceiveSessionData model)
        {
            try
            {
                await sessionService.UpdateSession(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(RemoveSession))]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveSession([FromBody, NotNull] RequestSession model)
        {
            try
            {
                await sessionService.RemoveSession(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }
    }

}