namespace PosterBoi.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentMessage { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
    }
}