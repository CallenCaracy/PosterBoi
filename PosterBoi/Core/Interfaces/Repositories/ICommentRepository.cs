using PosterBoi.Core.DTOs;
using PosterBoi.Core.Models;

namespace PosterBoi.Core.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<bool> CreateCommentAsync(Comment comment);
        Task<IEnumerable<CommentSummaryDto>> GetCommentsByPostIdAsync(int id, DateTime? after, int limit);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<bool> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}
