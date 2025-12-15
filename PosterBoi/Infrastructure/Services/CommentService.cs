using PosterBoi.Core.Configs;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Interfaces.Services;
using PosterBoi.Core.Models;
using PosterBoi.Infrastructure.Repositories;

namespace PosterBoi.Infrastructure.Services
{
    public class CommentService(ICommentRepository commentRepository) : ICommentService
    {
        private readonly ICommentRepository _commentRepository = commentRepository;

        public async Task<Result<bool>> CreateCommentAsync(CommentDto comment)
        {
            if (string.IsNullOrWhiteSpace(comment.CommentMessage))
                return Result<bool>.Fail("Comment must have a message");

            var createdComment = new Comment
            {
                ParentCommentId = comment.ParentCommentId,
                CommentMessage = comment.CommentMessage,
                ImgUrl = comment.ImgUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PostId = comment.PostId,
                UserId = comment.UserId,
            };

            var isCreated = await _commentRepository.CreateCommentAsync(createdComment);
            if (!isCreated)
                return Result<bool>.Fail("Failed to create comment.");

            return Result<bool>.Ok(isCreated);
        }

        public async Task<Result<IEnumerable<CommentSummaryDto>>> GetCommentsByPostIdAsync(int id, DateTime? after, int limit)
        {
            var comments = await _commentRepository.GetCommentsByPostIdAsync(id, after, limit);
            if (comments == null || !comments.Any())
                return Result<IEnumerable<CommentSummaryDto>>.Fail("No comments found.");
            return Result<IEnumerable<CommentSummaryDto>>.Ok(comments);
        }

        public async Task<Result<Comment>> GetCommentByIdAsync(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
                return Result<Comment>.Fail("Failed to fetch comment.");
            return Result<Comment>.Ok(comment);
        }

        public async Task<Result<bool>> DeleteCommentAsync(int id)
        {
            var isDeleted = await _commentRepository.DeleteCommentAsync(id);
            if (!isDeleted)
                return Result<bool>.Fail("Failed to delete comment.");
            return Result<bool>.Ok(isDeleted);
        }

        public async Task<Result<bool>> UpdateCommentAsync(int id, CommentDto comment)
        {
            var commentUpdated = await _commentRepository.GetCommentByIdAsync(id);
            if (commentUpdated == null)
                return Result<bool>.Fail("Comment not found.");

            if (commentUpdated.UserId != comment.UserId)
                return Result<bool>.Fail("Unauthorized.");

            commentUpdated.CommentMessage = comment.CommentMessage;
            commentUpdated.ImgUrl = comment.ImgUrl;
            commentUpdated.UpdatedAt = DateTime.UtcNow;

            var isUpdated = await _commentRepository.UpdateCommentAsync(commentUpdated);
            if (!isUpdated)
                return Result<bool>.Fail("Failed to update comment.");
            return Result<bool>.Ok(isUpdated);
        }
    }
}
