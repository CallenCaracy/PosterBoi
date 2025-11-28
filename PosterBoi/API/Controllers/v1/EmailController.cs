using Microsoft.AspNetCore.Mvc;
using PosterBoi.Core.Interfaces.Services;

namespace PosterBoi.API.Controllers.v1
{
    public class EmailController(IEmailService emailService) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService;

        [HttpPost("resendConfirmation")]
        public async Task<IActionResult> ResendComfirmationEmail([FromBody] string email)
        {
            var result = await _emailService.ResendConfirmationEmailAsync(email);
            if(!result.Success)
                return NotFound("Failed to resend confirmation email.");
            return Ok(result);
        }
    }
}
