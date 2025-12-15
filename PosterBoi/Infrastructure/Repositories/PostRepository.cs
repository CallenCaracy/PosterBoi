using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Models;
using PosterBoi.Infrastructure.Data;

namespace PosterBoi.Infrastructure.Repositories
{
    public class PostRepository(AppDbContext context, ILogger<PostRepository> logger) : IPostRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<PostRepository> _logger = logger;

        public async Task<bool> CreatePostAsync(Post post)
        {
            try
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add post for user {UserId}. Title: {Title}.", post.UserId, post.Title);
                return false;
            }
        }

        public async Task<IEnumerable<PostSummary>> GetAllPostsAsync(DateTime? after, int limit)
        {
            try
            {
                var query = _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Reactions)
                    .Include(p => p.Comments)
                    .OrderByDescending(p => p.CreatedAt)
                    .AsQueryable();

                if (after.HasValue)
                    query = query.Where(p => p.CreatedAt < after.Value);

                var posts = await query
                    .Select(p => new PostSummary
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        ImgUrl = p.ImgUrl,
                        UserId = p.UserId,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        ReactionCount = p.Reactions.Count,
                        ReactionSummary = p.Reactions
                            .GroupBy(r => r.Type)
                            .ToDictionary(g => g.Key, g => g.Count()),
                        CommentCount = p.Comments.Count,
                        User = p.User != null ? new UserSummaryDto
                        {
                            Name = p.User.Name,
                            Username = p.User.Username,
                            PfpUrl = p.User.PfpUrl,
                        } : null
                    })
                    .Take(limit)
                    .ToListAsync();

                return posts;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to fetch posts.");
                return [];
            }
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Posts.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch post by id: {Id}.", id);
                return null;
            }
        }

        public async Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                return await _context.Posts
                    .Where(p => p.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to post by userId: {UserId}.", userId);
                return [];
            }
        }

        public async Task<bool> UpdatePostAsync(Post post)
        {
            try
            {
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to update post by id: {Id}.", post.Id);
                return false;
            }
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null) return false;

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to delete post by id: {Id}.", id);
                return false;
            }
        }
    }
}
