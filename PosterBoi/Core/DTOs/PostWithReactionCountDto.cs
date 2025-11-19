using PosterBoi.Core.Models;

namespace PosterBoi.Core.DTOs
{
    public class PostWithReactionCountDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid UserId { get; set; }
        public UserSummaryDto User { get; set; } = null!;

        public int ReactionCount { get; set; }
        public int CommentCount { get; set; }
    }
}
