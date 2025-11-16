using PosterBoi.Core.Models;

namespace PosterBoi.Core.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<bool> CreateCommentAsync(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int id);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<bool> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}
