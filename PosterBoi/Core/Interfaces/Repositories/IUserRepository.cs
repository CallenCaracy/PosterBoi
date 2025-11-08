using PosterBoi.Core.Models;

namespace PosterBoi.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<bool> CreateUserAsync(User user);
    }
}
