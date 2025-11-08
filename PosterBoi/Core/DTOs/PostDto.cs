namespace PosterBoi.Core.DTOs
{
    public class PostDto
    {
        public string Title { get; set; } = null!;
        public string ImgUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
