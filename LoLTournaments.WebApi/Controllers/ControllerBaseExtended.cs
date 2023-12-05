using System;
using LoLTournaments.Application.Exceptions;
using LoLTournaments.Shared.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoLTournaments.WebApi.Controllers
{

    public abstract class ControllerBaseExtended : ControllerBase
    {
        protected IActionResult HandleException(Exception exception)
        {
            return exception switch
            {
                ForbiddenException => Forbid(exception.Message),
                ClientException => BadRequest(exception.Message),
                ValidationException => BadRequest(exception.Message),
                UnauthorizedHttpException => Unauthorized(exception.Message),
                NotFoundException => NotFound(exception.Message),
                _ => InternalServerError(exception),
            };
        }

        protected IActionResult InternalServerError(Exception exception)
        {
            DefaultSharedLogger.Error(exception);
            return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
        }
    }

}