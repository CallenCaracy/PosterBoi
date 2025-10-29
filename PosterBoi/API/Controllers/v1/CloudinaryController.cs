using Microsoft.AspNetCore.Mvc;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces;

namespace PosterBoi.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CloudinaryController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;

        public CloudinaryController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto([FromForm] FileUploadDto request)
        {
            if(request.File == null || request.File.Length == 0)
                return BadRequest("Invalid File");

            using var stream = request.File.OpenReadStream();
            var url = await _cloudinaryService.UploadAsync(stream, request.File.Name);
            return Ok(new { url });
        }
    }
}
