using Microsoft.EntityFrameworkCore;
using PosterBoi.Core.Configs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Models;
using PosterBoi.Infrastructure.Data;

namespace PosterBoi.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly AppDbContext _context;

        public SessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Session?> GetByTokenAsync(string token)
        {
            return await _context.Sessions.Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Token == token);
        }

        public async Task<bool> AddAsync(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Token == token);
            if (session == null)
                return false;

            session.IsRevoked = true;
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
