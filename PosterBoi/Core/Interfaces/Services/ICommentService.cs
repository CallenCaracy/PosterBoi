using PosterBoi.Core.Models;
using PosterBoi.Core.Configs;
using PosterBoi.Core.DTOs;

namespace PosterBoi.Core.Interfaces.Services
{
    public interface ICommentService
    {
        Task<Result<bool>> CreateCommentAsync(CommentDto comment);
        Task<Result<IEnumerable<CommentSummaryDto>>> GetCommentsByPostIdAsync(int id, DateTime? after, int limit);
        Task<Result<Comment>> GetCommentByIdAsync(int id);
        Task<Result<bool>> DeleteCommentAsync(int id);
        Task<Result<bool>> UpdateCommentAsync(int id, CommentDto comment);
    }
}