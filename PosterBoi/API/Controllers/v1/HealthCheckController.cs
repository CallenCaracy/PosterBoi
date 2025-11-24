using Microsoft.AspNetCore.Mvc;

namespace PosterBoi.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("healthCheck")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult HealthCheck()
        {
            return Ok("Healthy");
        }
    }
}
