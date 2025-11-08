namespace PosterBoi.Core.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime Created { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
