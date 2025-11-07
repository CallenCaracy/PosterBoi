using PosterBoi.Core.Models;

namespace PosterBoi.Core.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<bool> CreatePostAsync(Post post);
        Task<IEnumerable<Post>> GetAllPostsAsync(DateTime? after, int limit);
        Task<Post?> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(int Id);
    }
}