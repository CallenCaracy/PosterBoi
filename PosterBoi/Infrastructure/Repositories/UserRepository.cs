using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Infrastructure.Data;
using PosterBoi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace PosterBoi.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context, ILogger<UserRepository> logger) : IUserRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<UserRepository> _logger = logger;

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to fetch by email: {Email}", email);
                return null;
            }
        }

        public async Task<User?> GetByTokenAsync(string token)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Token == token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch by token: {Token}", token);
                return null;
            }
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch by id: {Id}", id);
                return null;
            }
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user: {Id}", user.Id);
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user: {Id}", user.Id);
                return false;
            }
        }
    }
}
