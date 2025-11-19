using PosterBoi.Core.Models;

namespace PosterBoi.Core.DTOs
{
    public class ReactionDto
    {
        public int PostId { get; set; }
        public Guid UserId { get; set; }
        public ReactionType Type { get; set; }
    }
}