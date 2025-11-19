using PosterBoi.Core.Models;

namespace PosterBoi.Core.Interfaces.Repositories
{
    public interface IReactionRepository
    {
        Task<Reaction?> GetReactionByIdAsync(int id);
        Task<int> GetCountByPostIdAsync(int postId);
        Task<Dictionary<ReactionType, int>> GetCountByTypeAsync(int postId);
        Task<bool> AddReactionAsync(Reaction reaction);
        Task<bool> UpdateReactionAsync(Reaction reaction);
        Task<bool> RemoveReactionAsync(int postId, Guid userId);
    }
}