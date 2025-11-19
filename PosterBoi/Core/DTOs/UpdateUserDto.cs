namespace PosterBoi.Core.DTOs
{
    public class UpdateUserDto
    {
        public string Username { get; set; } = null!;
        public string? PfpUrl { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
