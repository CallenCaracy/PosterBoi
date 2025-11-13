using System.ComponentModel.DataAnnotations.Schema;

namespace PosterBoi.Core.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = [];
    }
}