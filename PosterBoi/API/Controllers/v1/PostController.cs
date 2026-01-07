using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Interfaces.Services;

namespace PosterBoi.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

        [HttpPost("createPost")]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] PostDto request)
        {
            var result = await _postService.CreatePostAsync(request);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpGet("getAllPosts")]
        public async Task<IActionResult> GetAllPosts(DateTime? after, int limit, Guid? userId)
        {
            var result = await _postService.GetAllPostsAsync(after, limit, userId);
            if (!result.Success) return NotFound(result.Message);
            return Ok(result);
        }

        [HttpGet("getPostById/{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var result = await _postService.GetPostByIdAsync(id);
            if (!result.Success) return NotFound(result.Message);
            return Ok(result);
        }

        [HttpGet("getPostsByUserID/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(Guid userId)
        {
            var result = await _postService.GetPostsByUserIdAsync(userId);
            if (!result.Success) return NotFound(result.Message);
            return Ok(result);
        }

        [HttpPut("updatePost/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost(int id,[FromBody] PostDto request)
        {
            var result = await _postService.UpdatePostAsync(id, request);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpDelete("deletePost/{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _postService.DeletePostAsync(id);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result);
        }
    }
}
