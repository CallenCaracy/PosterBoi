namespace PosterBoi.Core.DTOs
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = null!;
        public string Username { get; set; } = string.Empty;
        public string? PfpUrl { get; set; }
        public string? CoverPfpUrl { get; set; }
        public string? Bio { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
