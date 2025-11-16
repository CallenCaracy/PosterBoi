using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Interfaces.Services;
using PosterBoi.Core.Models;
using PosterBoi.Core.Configs;
using PosterBoi.Infrastructure.Helpers;

namespace PosterBoi.Infrastructure.Services
{
    public class SessionService(ISessionRepository sessionRepository, JwtHelper jwtHelper) : ISessionService
    {
        private readonly ISessionRepository _sessionRepository = sessionRepository;
        private readonly JwtHelper _jwtHelper = jwtHelper;

        public async Task<Jwt?> GenerateTokens(User user)
        {
            var accessToken = _jwtHelper.GenerateJwtToken(user);
            var refreshToken = JwtHelper.GenerateRefreshToken();

            var session = new Session
            {
                Token = refreshToken,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(30),
                Created = DateTime.UtcNow,
                IsRevoked = false
            };

            var isAdded = await _sessionRepository.AddAsync(session);
            if (!isAdded)
                return null;

            return new Jwt { 
                AccessToken = accessToken, 
                RefreshToken = refreshToken 
            }; 
        }

        public async Task<Result<Jwt>> RefreshTokensAsync(string refreshToken)
        {
            var session = await _sessionRepository.GetByTokenAsync(refreshToken);

            if (session == null || session.IsRevoked || session.Expires < DateTime.UtcNow)
                return Result<Jwt>.Fail("Invalid or expired refresh token.");

            var user = session.User;
            session.IsRevoked = true;

            var isUpdated = await _sessionRepository.UpdateAsync(session);
            if (!isUpdated)
                return Result<Jwt>.Fail("Failed to update refresh token.");

            var tokens = await GenerateTokens(user);
            if (tokens == null)
                return Result<Jwt>.Fail("Failed to generate tokens.");

            return Result<Jwt>.Ok(tokens);
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            var isRevoked = await _sessionRepository.RevokeTokenAsync(refreshToken);
            if (!isRevoked)
                return false;

            return true;
        }
    }
}
