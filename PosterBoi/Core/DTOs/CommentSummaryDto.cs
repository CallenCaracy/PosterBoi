using PosterBoi.Core.Models;

namespace PosterBoi.Core.DTOs
{
    public class CommentSummaryDto
    {
        public int Id { get; set; }
        public string CommentMessage { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public DateTime UpdatedAt { get; set; } = DateTime.MinValue;

        public Guid UserId { get; set; }
        public UserSummaryDto User { get; set; } = null!;

        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }
        public ICollection<CommentSummaryDto> ChildComments { get; set; } = [];
    }
}
