using PosterBoi.Core.DTOs;
using PosterBoi.Core.Entities;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Interfaces.Services;

namespace PosterBoi.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<bool> CreatePostAsync(PostDto request)
        {
            var post = new Post()
            {
                Title = request.Title,
                Description = request.Description,
                ImgUrl = request.ImgUrl,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            return await _postRepository.CreatePostAsync(post);
        }

        public async Task<bool> UpdatePostAsync(int Id, PostDto request)
        {
            var postUpdated = await _postRepository.GetByIdAsync(Id);
            if (postUpdated == null || request.UserId != postUpdated.UserId) return false;

            postUpdated.Title = request.Title;
            postUpdated.Description = request.Description;
            postUpdated.UpdatedAt = DateTime.UtcNow;
            postUpdated.ImgUrl = request.ImgUrl;

            return await _postRepository.UpdatePostAsync(postUpdated);
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _postRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _postRepository.GetAllPostsAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId)
        {
            return await _postRepository.GetByUserIdAsync(userId);
        }
        public async Task<bool> DeletePostAsync(int id)
        {
            return await _postRepository.DeletePostAsync(id);
        }

    }
}
