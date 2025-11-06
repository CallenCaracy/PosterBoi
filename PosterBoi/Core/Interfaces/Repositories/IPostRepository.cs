using PosterBoi.Core.DTOs;
using PosterBoi.Core.Entities;

namespace PosterBoi.Core.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<bool> CreatePostAsync(Post post);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post?> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(int Id);
    }
}