﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using LoLTournaments.Application.Exceptions;
using LoLTournaments.Application.Services;
using LoLTournaments.Shared.Common;
using LoLTournaments.Shared.Models;
using LoLTournaments.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoLTournaments.WebApi.Controllers
{

    [Route("api/" + VersionInfo.APIVersion + "/[controller]")]
    [ApiController]
    [Authorize]
    public class LobbyController : ControllerBaseExtended
    {
        private readonly ILobbyService lobbyService;

        public LobbyController(ILobbyService lobbyService)
        {
            this.lobbyService = lobbyService;
        }

        [HttpGet]
        [Route(nameof(GetRooms))]
        [AllowAnonymous]
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                return Ok(await lobbyService.GetRooms());
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(GetRoom))]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoom([FromBody, NotNull] RequestSessionData model)
        {
            try
            {
                return Ok(await lobbyService.GetRoom(model));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(GetRoomData))]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomData([FromBody, NotNull] RequestSessionData model)
        {
            try
            {
                return Ok(await lobbyService.GetRoomData(model));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(PostRoomData))]
        [AllowAnonymous]
        public async Task<IActionResult> PostRoomData([FromBody, NotNull] ReceiveSessionData model)
        {
            try
            {
                await lobbyService.SetRoomData(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(RemoveRoom))]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveRoom([FromBody, NotNull] RequestSessionData model)
        {
            try
            {
                await lobbyService.RemoveRoom(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(UpdateRoom))]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateRoom([FromBody, NotNull] ReceiveSessionData model)
        {
            try
            {
                await lobbyService.UpdateRoom(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(RemoveRegistration))]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveRegistration([FromBody, NotNull] ReceiveSessionData model)
        {
            try
            {
                await lobbyService.RemoveRegistration(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(UpdateRegistration))]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateRegistration([FromBody, NotNull] ReceiveSessionData model)
        {
            try
            {
                await lobbyService.UpdateRegistration(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(RemoveAcception))]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveAcception([FromBody, NotNull] ReceiveSessionData model)
        {
            try
            {
                await lobbyService.RemoveAcception(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [HttpPost]
        [Route(nameof(UpdateAcception))]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAcception([FromBody, NotNull] ReceiveSessionData model)
        {
            try
            {
                await lobbyService.UpdateAcception(model);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }
    }

}