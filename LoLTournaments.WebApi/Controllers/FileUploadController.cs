using LoLTournaments.Shared.Models;
using LoLTournaments.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoLTournaments.WebApi.Controllers
{

    [ApiController]
    [Route("api/" + VersionInfo.APIVersion + "/[controller]")]
    [Authorize]
    public class FileUploadController : ControllerBaseExtended
    {
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromBody] FileUploadModel model)
        {
            if (model is not {IsValid: true})
                return BadRequest("No file was uploaded.");

            // Define the folder where you want to save the uploaded files
            var folder = "Uploads";
            var uploadPath = $"wwwroot/{folder}";
            var fullFileName = $"{model.Name}_{Guid.NewGuid()}{model.Extension}";
            var fullUploadPath = Path.Combine(Directory.GetCurrentDirectory(), uploadPath);
            
            // Create the folder if it doesn't exist
            if (!Directory.Exists(fullUploadPath))
                Directory.CreateDirectory(fullUploadPath);

            // Generate a unique filename for the uploaded file
            var filePath = Path.Combine(fullUploadPath, fullFileName);

            // Save the uploaded file to the specified path
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.WriteAsync(model.Data);
                fileStream.Close();
            }

            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}/{folder}/{fullFileName}";
            return Ok(new FileUrl{Url = url});
        }
    }

}