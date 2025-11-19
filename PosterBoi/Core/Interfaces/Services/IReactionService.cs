using PosterBoi.Core.Configs;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Models;

namespace PosterBoi.Core.Interfaces.Services
{
    public interface IReactionService
    {
        Task<int> GetCountByPostIdAsync(int postId);
        Task<Result<Dictionary<ReactionType, int>?>> GetCountByTypeAsync(int postId);
        Task<Result<bool>> AddReactionAsync(ReactionDto request);
        Task<Result<bool>> UpdateReactionAsync(int id, ReactionDto request);
        Task<Result<bool>> RemoveReactionAsync(int postId, Guid userId);
    }
}