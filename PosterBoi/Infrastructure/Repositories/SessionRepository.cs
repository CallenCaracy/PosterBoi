using Microsoft.EntityFrameworkCore;
using PosterBoi.Core.Configs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Models;
using PosterBoi.Infrastructure.Data;

namespace PosterBoi.Infrastructure.Repositories
{
    public class SessionRepository(AppDbContext context, ILogger<SessionRepository> logger) : ISessionRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<SessionRepository> _logger = logger;

        public async Task<Session?> GetByTokenAsync(string token)
        {
            try
            {
                return await _context.Sessions.Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Token == token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch token: {Token}.", token);
                return null;
            }
        }

        public async Task<bool> AddAsync(Session session)
        {
            try
            {
                await _context.Sessions.AddAsync(session);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to add session: {Token}", session.Token);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Session session)
        {
            try
            {
                _context.Sessions.Update(session);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update session: {Token}", session.Token);
                return false;
            }
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            try
            {
                var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Token == token);
                if (session == null)
                    return false;
                
                session.IsRevoked = true;
            
                _context.Sessions.Update(session);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to revoke token: {Token}.", token);
                return false;
            }
        }
    }
}
