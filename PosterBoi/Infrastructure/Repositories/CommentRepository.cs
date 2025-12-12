using Microsoft.EntityFrameworkCore;
using PosterBoi.Core.Configs;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Models;
using PosterBoi.Infrastructure.Data;

namespace PosterBoi.Infrastructure.Repositories
{
    public class CommentRepository(AppDbContext context, ILogger<CommentRepository> logger) : ICommentRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<CommentRepository> _logger = logger;

        public async Task<bool> CreateCommentAsync(Comment comment)
        {
            try
            {
                if (comment.ParentCommentId != null)
                {
                    var parent = await _context.Comments
                        .Select(c => new { c.Id, c.ParentCommentId })
                        .FirstOrDefaultAsync(c => c.Id == comment.ParentCommentId);

                    if (parent == null || parent.ParentCommentId != null)
                        return false;
                }

                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to create comment: {Id}", comment.Id);
                return false;
            }
        }

        public async Task<IEnumerable<CommentSummaryDto>> GetCommentsByPostIdAsync(int id, DateTime? after, int limit)
        {
            try
            {
                var query = _context.Comments
                    .Where(c => c.PostId == id && c.ParentCommentId == null)
                    .OrderByDescending(c => c.CreatedAt)
                    .AsQueryable();

                if (after.HasValue)
                    query = query.Where(c => c.CreatedAt < after.Value);

                var comments = await query
                    .Take(limit)
                    .Select(c => new CommentSummaryDto
                    {
                        Id = c.Id,
                        CommentMessage = c.CommentMessage,
                        ImgUrl = c.ImgUrl,
                        PostId = c.PostId,
                        UserId = c.UserId,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        ParentCommentId = c.ParentCommentId,

                        User = new UserSummaryDto
                        {
                            Name = c.User.Name,
                            Username = c.User.Username,
                            PfpUrl = c.User.PfpUrl
                        },

                        ChildComments = c.ChildComments
                            .OrderByDescending(cc => cc.CreatedAt)
                            .Select(cc => new CommentSummaryDto
                            {
                                Id = cc.Id,
                                CommentMessage = cc.CommentMessage,
                                ImgUrl = cc.ImgUrl,
                                PostId = cc.PostId,
                                UserId = cc.UserId,
                                CreatedAt = cc.CreatedAt,
                                UpdatedAt = cc.UpdatedAt,
                                ParentCommentId = cc.ParentCommentId,

                                User = new UserSummaryDto
                                {
                                    Name = cc.User.Name,
                                    Username = cc.User.Username,
                                    PfpUrl = cc.User.PfpUrl
                                }
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch comments by post id: {PostId}", id);
                return [];
            }
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            try
            {
                return await _context.Comments.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch comment by id: {Id}", id);
                return null;
            }
        }

        public async Task<bool> UpdateCommentAsync(Comment comment)
        {
            try
            {
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to update comment: {Id}", comment.Id);
                return false;
            }
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            try
            {
                var comment = await _context.Comments
                    .Include(c => c.ChildComments)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (comment == null)
                    return false;

                foreach (var child in comment.ChildComments.ToList())
                {
                    _context.Comments.Remove(child);
                }
                
                _context.Comments.Remove(comment);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete comment: {Id}", id);
                return false;
            }
        }
    }
}
