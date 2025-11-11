using PosterBoi.Core.Models;

namespace PosterBoi.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<bool> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        //Task<Result<IEnumerable<User>>> GetFriendsAsync(); Later when we do this
    }
}
