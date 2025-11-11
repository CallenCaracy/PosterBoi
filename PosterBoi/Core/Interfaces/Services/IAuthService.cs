using PosterBoi.Core.Configs;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Models;

namespace PosterBoi.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<Guid>> RegisterUserAsync(SignInDto request);
        Task<Result<Jwt>> LoginAsync(LoginDto request);
        Task<Result<bool>> LogoutAsync(string refreshToken);
        Task<Result<bool>> UpdateUserAsync(Guid userId, UpdateUserDto request);
        //Task<Result<IEnumerable<User>>> GetFriendsAsync(); Later when we do this
        Task<Result<User>> GetUserByIdAsync(Guid userId);
    }
}
