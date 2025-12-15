using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Services;
using PosterBoi.Core.Models;

namespace PosterBoi.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(SignInDto request)
        {
            var result = await _authService.RegisterUserAsync(request);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var result = await _authService.LoginAsync(request);
            if(!result.Success)
                return NotFound(result.Message);

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

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutDto request)
        {
            var result = await _authService.LogoutAsync(request.RefreshToken);
            if (!result.Success)
                return BadRequest("Invalid token");

            return Ok(result);
        }

        [HttpPatch("updateUser/{UserId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid UserId, UpdateUserDto request)
        {
            var result = await _authService.UpdateUserAsync(UserId, request);
            if (!result.Success)
                return BadRequest("Failed to update user.");

            return Ok(result);
        }

        [HttpGet("getUserById/{UserId}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid UserId)
        {
            var result = await _authService.GetUserByIdAsync(UserId);
            if (!result.Success)
                return BadRequest("Failed to update user.");

            return Ok(result);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            var result = await _authService.ConfirmUserAsync(token);
            if (!result.Success)
                return BadRequest("Error confirming email.");

            return Ok("Account confirmed.");
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _authService.ForgotPasswordAsync(email);
            if (!result.Success)
                return BadRequest("Error initiating recovery email.");

            return Ok("Account recovery sent.");
        }

        [HttpPost("recovery")]
        public async Task<IActionResult> RecoveryAccount([FromBody] RecoveryRequestDto request)
        {
            var result = await _authService.RecoverAccountAsync(request.Token, request.NewPassword);
            if (!result.Success)
                return BadRequest("Error recovering email.");

            return Ok("Account recovered successfully.");
        }
    }
}
