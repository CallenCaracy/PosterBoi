using PosterBoi.Core.Models;

namespace PosterBoi.Core.DTOs
{
    public class CommentDto
    {
        public string CommentMessage { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }
        public int? ParentCommentId { get; set; }
        public Guid UserId { get; set; }
        public int PostId { get; set; }
    }
}
