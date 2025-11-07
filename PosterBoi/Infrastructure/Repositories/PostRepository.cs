using Microsoft.EntityFrameworkCore;
using PosterBoi.Core.Models;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Infrastructure.Data;

namespace PosterBoi.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

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

        public async Task<Post?> GetByIdAsync(int Id)
        {
            return await _context.Posts.FindAsync(Id);
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

        public async Task<bool> DeletePostAsync(int Id)
        {
            var post = await _context.Posts.FindAsync(Id);
            if (post == null) return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
