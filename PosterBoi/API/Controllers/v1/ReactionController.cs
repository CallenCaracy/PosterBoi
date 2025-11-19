using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Services;

namespace PosterBoi.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReactionController(IReactionService reactionService) : ControllerBase
    {
        private readonly IReactionService _reactionService = reactionService;

        [HttpGet("getAllReactionCount")]
        public async Task<IActionResult> GetAllReactionCount(int postId)
        {
            return Ok(await _reactionService.GetCountByPostIdAsync(postId));
        }

        [HttpGet("getReactionCountByTypes")]
        public async Task<IActionResult> GetReactionCountByTypes(int postId)
        {
            var result = await _reactionService.GetCountByTypeAsync(postId);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result);
        }

        [HttpPost("addReaction")]
        [Authorize]
        public async Task<IActionResult> AddReaction(ReactionDto request)
        {
            var result = await _reactionService.AddReactionAsync(request);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpPatch("updateReaction/{id}")]
        public async Task<IActionResult> UpdateReaction(int id, [FromBody]ReactionDto reaction)
        {
            var result = await _reactionService.UpdateReactionAsync(id, reaction);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpDelete("removeReaction")]
        [Authorize]
        public async Task<IActionResult> RemoveReaction(int postId, Guid userId)
        {
            var result = await _reactionService.RemoveReactionAsync(postId, userId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
    }
}
