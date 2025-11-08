using PosterBoi.Core.Configs;
using PosterBoi.Core.DTOs;

namespace PosterBoi.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<Guid>> RegisterUserAsync(SignInDto request);
        Task<Result<string>> LoginAsync(LoginDto request);
    }
}
