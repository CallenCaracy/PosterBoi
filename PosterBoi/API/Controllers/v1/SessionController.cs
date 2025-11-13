using Microsoft.AspNetCore.Mvc;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Services;

namespace PosterBoi.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SessionController(ISessionService sessionService) : ControllerBase
    {
        private readonly ISessionService _sessionService = sessionService;

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshSessionDto request)
        {
            var result = await _sessionService.RefreshTokensAsync(request.RefreshToken);
            if (!result.Success)
                return Unauthorized(result.Message);

            var jwt = result.Data;
            if (jwt == null)
                return NotFound(result.Message);

            Response.Cookies.Append("refreshToken", jwt.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            return Ok(new
            {
                accessToken = jwt.AccessToken,
            });
        }
    }
}
