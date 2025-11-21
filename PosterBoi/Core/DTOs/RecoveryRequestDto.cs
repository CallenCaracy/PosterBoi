namespace PosterBoi.Core.DTOs
{
    public class RecoveryRequestDto
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
