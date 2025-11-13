namespace PosterBoi.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentMessage { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public DateTime UpdatedAt { get; set; } = DateTime.MinValue;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
        
        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> ChildComments { get; set; } = [];
    }
}