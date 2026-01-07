using PosterBoi.Core.Models;

namespace PosterBoi.Core.DTOs
{
    public class ReactionSummaryDto
    {
        public Dictionary<ReactionType, int> ReactionTypes { get; set; } = null!;
        public int? UserReactionId { get; set; }
        public string? UserReactionName { get; set; }
    }
}
