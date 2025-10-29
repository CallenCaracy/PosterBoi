namespace PosterBoi.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentMessage { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public int UserId { get; set; } 

    }
}
