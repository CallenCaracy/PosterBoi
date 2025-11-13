using Microsoft.EntityFrameworkCore;
using PosterBoi.Core.Models;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Infrastructure.Data;

namespace PosterBoi.Infrastructure.Repositories
{
    public class PostRepository(AppDbContext context) : IPostRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<bool> CreatePostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync(DateTime? after, int limit)
        {
            var query = _context.Posts.OrderByDescending(post => post.CreatedAt).AsQueryable();
            if (after.HasValue)
                query = query.Where(p => p.CreatedAt < after.Value);
            return await query.Take(limit).ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
