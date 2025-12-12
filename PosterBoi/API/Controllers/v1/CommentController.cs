using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Models;
using PosterBoi.Core.Interfaces.Services;

namespace PosterBoi.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class CommentController(ICommentService commentService) : ControllerBase
    {
        private readonly ICommentService _commentService = commentService;

        [HttpPost("createComment")]
        [Authorize]
        public async Task<IActionResult> CreateComment(CommentDto request)
        {
            var result = await _commentService.CreateCommentAsync(request);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpGet("getCommentsByPostId/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId, DateTime? after, int limit)
        {
            var result = await _commentService.GetCommentsByPostIdAsync(postId, after, limit);
            if (!result.Success) return BadRequest(result.Message); 
            return Ok(result);
        }

        [HttpGet("getCommentById{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var result = await _commentService.GetCommentByIdAsync(id);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpPatch("updateComment/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto request)
        {
            var result = await _commentService.UpdateCommentAsync(id, request);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpDelete("deleteComment/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result);
        }
    }
}
