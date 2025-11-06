using PosterBoi.Core.DTOs;
using PosterBoi.Core.Entities;

namespace PosterBoi.Core.Interfaces.Services
{
    public interface IPostService
    {
        Task<bool> CreatePostAsync(PostDto request);
        Task<bool> UpdatePostAsync(int id, PostDto request);
    }
}
