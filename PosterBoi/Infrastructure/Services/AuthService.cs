using PosterBoi.Core.Interfaces.Services;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Models;
using PosterBoi.Core.Configs;
using PosterBoi.Infrastructure.Helpers;

namespace PosterBoi.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionService _sessionService;

        public AuthService(IUserRepository userRepository, ISessionService sessionService)
        {
            _userRepository = userRepository;
            _sessionService = sessionService;
        }

        public async Task<Result<Guid>> RegisterUserAsync(SignInDto request)
        {
            var existing = await _userRepository.GetByEmailAsync(request.Email);
            if (existing != null)
                return Result<Guid>.Fail("Email already registered, please login instead.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Username,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
            };

            await _userRepository.CreateUserAsync(user);
            return Result<Guid>.Ok(user.Id);
        }

        public async Task<Result<Jwt>> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return Result<Jwt>.Fail("Invalid credentials.");

            var token = await _sessionService.GenerateTokens(user);
            return Result<Jwt>.Ok(token);
        }

        public async Task<Result<bool>> LogoutAsync(string refreshToken)
        {
            await _sessionService.RevokeRefreshTokenAsync(refreshToken);
            return Result<bool>.Ok(true);
        }
    }
}
