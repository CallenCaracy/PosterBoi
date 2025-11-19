using PosterBoi.Core.Configs;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Interfaces.Services;
using PosterBoi.Core.Models;

namespace PosterBoi.Infrastructure.Services
{
    public class ReactionService(IReactionRepository reactionRepository) : IReactionService
    {
        private readonly IReactionRepository _reactionRepository = reactionRepository;

        public async Task<int> GetCountByPostIdAsync(int postId)
        {
            return await _reactionRepository.GetCountByPostIdAsync(postId);
        }

        public async Task<Result<Dictionary<ReactionType, int>?>> GetCountByTypeAsync(int postId)
        {
            var counts = await _reactionRepository.GetCountByTypeAsync(postId);
            if (counts == null)
                return Result<Dictionary<ReactionType, int>?>.Fail("Failed to fetch count by types.");
            return Result<Dictionary<ReactionType, int>?>.Ok(counts);
        }

        public async Task<Result<bool>> AddReactionAsync(ReactionDto request)
        {
            var reaction = new Reaction
            {
                PostId = request.PostId,
                UserId = request.UserId,
                Type = request.Type,
                ReactedAt = DateTime.UtcNow,
            };

            var isAdded = await _reactionRepository.AddReactionAsync(reaction);
            if (!isAdded)
                return Result<bool>.Fail("Failed to add reaction.");
            return Result<bool>.Ok(isAdded);
        }

        public async Task<Result<bool>> UpdateReactionAsync(int id, ReactionDto request)
        {
            var reaction = await _reactionRepository.GetReactionByIdAsync(id);
            if (reaction == null)
                return Result<bool>.Fail("Failed to fetch reaction.");

            if (reaction.UserId != request.UserId)
                return Result<bool>.Fail("Unauthorized to update this reaction.");

            reaction.Type = request.Type;
            reaction.ReactedAt = DateTime.UtcNow;

            var isUpdated = await _reactionRepository.UpdateReactionAsync(reaction);
            if (!isUpdated)
                return Result<bool>.Fail("Failed to update reaction.");
            return Result<bool>.Ok(isUpdated);
        }

        public async Task<Result<bool>> RemoveReactionAsync(int postId, Guid userId)
        {
            var isRemoved = await _reactionRepository.RemoveReactionAsync(postId, userId);
            if (!isRemoved)
                return Result<bool>.Fail("Failed to remove reaction.");
            return Result<bool>.Ok(isRemoved);
        }
    }
}
