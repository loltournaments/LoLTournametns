using LoLTournaments.Shared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace LoLTournaments.WebApi.Controllers
{

    public abstract class SharedController : ControllerBase
    {
        protected bool IsValidVersion(string clientVersion, string serverVersion, out IActionResult versionResult)
        {
            if (clientVersion.ConvertVersion() >= serverVersion.ConvertVersion())
            {
                versionResult = Ok();
                return true;
            }
            
            versionResult = BadRequest($"Install the latest version : {serverVersion}");
            return false;
        }
    }

}