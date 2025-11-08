using PosterBoi.Core.Configs;
using PosterBoi.Core.Models;

namespace PosterBoi.Core.Interfaces.Repositories
{
    public interface ISessionRepository
    {
        Task<Session?> GetByTokenAsync(string token);
        Task<bool> AddAsync(Session session);
        Task<bool> UpdateAsync(Session session);
        Task<bool> RevokeTokenAsync(string token);
    }
}