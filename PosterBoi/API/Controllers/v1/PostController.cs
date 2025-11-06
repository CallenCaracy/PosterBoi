using Microsoft.AspNetCore.Mvc;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Services;

namespace PosterBoi.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("create_post")]
        public async Task<IActionResult> CreatePost([FromBody] PostDto request)
        {
            var isPostCreated = await _postService.CreatePostAsync(request);
            if (!isPostCreated) return BadRequest("Failed to create post.");
            return Ok(isPostCreated);
        } 
    }
}
