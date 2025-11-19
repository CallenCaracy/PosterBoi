using Microsoft.EntityFrameworkCore;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Models;
using PosterBoi.Infrastructure.Data;

namespace PosterBoi.Infrastructure.Repositories
{
    public class ReactionRepository(AppDbContext context, ILogger<ReactionRepository> logger) : IReactionRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<ReactionRepository> _logger = logger;

        public async Task<Reaction?> GetReactionByIdAsync(int id)
        {
            try
            {
                return await _context.Reactions.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch reaction with id: {Id}", id);
                return null;
            }
        }

        public async Task<int> GetCountByPostIdAsync(int postId)
        {
            try
            {
                var count = await _context.Reactions.CountAsync(p => p.PostId == postId);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch number of reactions on post Id: {PostId}", postId);
                return 0;
            }
        }

        public async Task<Dictionary<ReactionType, int>> GetCountByTypeAsync(int postId)
        {
            try
            {
                var reactions = await _context.Reactions
                    .Where(r => r.PostId == postId)
                    .GroupBy(r => r.Type)
                    .Select(g => new { Type = g.Key, Count = g.Count() })
                    .ToListAsync();

                var countByReactionType = reactions.ToDictionary(r => r.Type, r => r.Count);
                return countByReactionType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch number of reactions by type on post Id: {PostId}", postId);
                return [];
            }
        }
        public async Task<bool> AddReactionAsync(Reaction reaction)
        {
            try
            {
                var exists = await _context.Reactions
                    .AnyAsync(r => r.PostId == reaction.PostId && r.UserId == reaction.UserId);

                if (exists)
                    return false;

                _context.Reactions.Add(reaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add reaction with Id: {Id}", reaction.Id);
                return false;
            }
        }

        public async Task<bool> UpdateReactionAsync(Reaction reaction)
        {
            try
            {
                _context.Reactions.Update(reaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update reaction with id: {Id}", reaction.Id);
                return false;
            }
        }

        public async Task<bool> RemoveReactionAsync(int postId, Guid userId)
        {
            try
            {
                var reaction = await _context.Reactions
                .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);

                if (reaction == null) return false;

                _context.Reactions.Remove(reaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove reaction with post Id: {PostId}, user Id: {UserId}", postId, userId);
                return false;
            }
        }
    }
}