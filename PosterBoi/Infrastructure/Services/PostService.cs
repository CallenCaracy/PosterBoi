using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PosterBoi.Core.Configs;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Interfaces.Services;
using PosterBoi.Core.Models;

namespace PosterBoi.Infrastructure.Services
{
    public class PostService(IPostRepository postRepository) : IPostService
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<Result<Post>> CreatePostAsync(PostDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                return Result<Post>.Fail("Title Required,");

            var post = new Post()
            {
                Title = request.Title,
                Description = request.Description,
                ImgUrl = request.ImgUrl,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var isCreated = await _postRepository.CreatePostAsync(post);
            if (!isCreated)
                return Result<Post>.Fail("Failed to create post.");

            return Result<Post>.Ok(post);
        }

        public async Task<Result<Post>> UpdatePostAsync(int id, PostDto request)
        {
            var postUpdated = await _postRepository.GetByIdAsync(id);
            if (postUpdated == null || request.UserId != postUpdated.UserId) 
                return Result<Post>.Fail("Post to update does not exist.");

            postUpdated.Title = request.Title;
            postUpdated.Description = request.Description;
            postUpdated.UpdatedAt = DateTime.UtcNow;
            postUpdated.ImgUrl = request.ImgUrl;

            var isUpdated = await _postRepository.UpdatePostAsync(postUpdated);
            if(!isUpdated)
                return Result<Post>.Fail("Failed to save post updates.");

            return Result<Post>.Ok(postUpdated);
        }

        public async Task<Result<Post?>> GetPostByIdAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
                return Result<Post?>.Fail("Failed to fetch post.");

            return Result<Post?>.Ok(post);
        }

        public async Task<Result<IEnumerable<PostWithReactionCountDto>>> GetAllPostsAsync(DateTime? after, int limit)
        {
            var posts = await _postRepository.GetAllPostsAsync(after, limit);
            if (posts == null || !posts.Any())
                return Result<IEnumerable<PostWithReactionCountDto>>.Fail("No posts found.");
            return Result<IEnumerable<PostWithReactionCountDto>>.Ok(posts);
        }

        public async Task<Result<IEnumerable<Post>>> GetPostsByUserIdAsync(Guid userId)
        {
            var post = await _postRepository.GetByUserIdAsync(userId);
            if (post == null || !post.Any())
                return Result<IEnumerable<Post>>.Fail("No post found.");
            return Result<IEnumerable<Post>>.Ok(post);
        }
        public async Task<Result<bool>> DeletePostAsync(int id)
        {
            var isDeleted = await _postRepository.DeletePostAsync(id);
            if (!isDeleted)
                return Result<bool>.Fail("Failed to delete post.");
            return Result<bool>.Ok(isDeleted);
        }

    }
}
