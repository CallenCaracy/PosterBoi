using System.ComponentModel.DataAnnotations;

namespace PosterBoi.Core.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? PfpUrl { get; set; }
        public bool? Gender { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        // Add Chat and Messages later
    }
}