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
        Task<Result<IEnumerable<PostSummaryDto>>> GetAllPostsAsync(DateTime? after, int limit, Guid? userId);
        Task<Result<IEnumerable<Post>>> GetPostsByUserIdAsync(Guid userId);
        Task<Result<bool>> DeletePostAsync(int id);
    }
}
