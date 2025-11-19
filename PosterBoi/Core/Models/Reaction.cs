namespace PosterBoi.Core.Models
{
    public class Reaction
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Guid UserId { get; set; }
        public ReactionType Type { get; set; }
        public DateTime ReactedAt { get; set; }
    }

    public enum ReactionType
    {
        Like,
        Heart,
        Haha,
        Wow,
        Sad,
        Angry
    }
}