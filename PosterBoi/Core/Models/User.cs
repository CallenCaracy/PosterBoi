using System.ComponentModel.DataAnnotations;

namespace PosterBoi.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastActiveAt { get; set; }

        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        // Add Chat and Messages later
    }
}
