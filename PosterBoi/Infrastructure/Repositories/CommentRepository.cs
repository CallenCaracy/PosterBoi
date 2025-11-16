using Microsoft.EntityFrameworkCore;
using PosterBoi.Core.Configs;
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

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int id)
        {
            try
            {
                var comments = await _context.Comments
                    .Where(c => c.PostId == id && c.ParentCommentId == null)
                    .Include(c => c.ChildComments)
                    .ToListAsync();
                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch comments by post id: {Id}", id);
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
