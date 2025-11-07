using PosterBoi.Core.DTOs;
using PosterBoi.Core.Configs;
using PosterBoi.Core.Models;

namespace PosterBoi.Core.Interfaces.Services
{
    public interface IPostService
    {
        Task<Result<Post>> CreatePostAsync(PostDto request);
        Task<Result<Post>> UpdatePostAsync(int id, PostDto request);
        Task<Result<Post?>> GetPostByIdAsync(int id);
        Task<Result<IEnumerable<Post>>> GetAllPostsAsync(DateTime? after, int limit);
        Task<Result<IEnumerable<Post>>> GetPostsByUserIdAsync(Guid userId);
        Task<Result<bool>> DeletePostAsync(int id);
    }
}
