using PosterBoi.Core.Models;
using PosterBoi.Core.Configs;

namespace PosterBoi.Core.Interfaces.Services
{
    public interface ISessionService
    {
        Task<Jwt?> GenerateTokens(User user);
        Task<Result<string>> RefreshTokensAsync(string refreshToken);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    }
}
