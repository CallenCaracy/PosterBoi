namespace PosterBoi.Core.Models
{
    public class Jwt
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
