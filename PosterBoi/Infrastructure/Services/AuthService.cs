using Azure.Core;
using PosterBoi.Core.Configs;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Interfaces.Services;
using PosterBoi.Core.Models;
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
                PfpUrl = request.PfpUrl,
                Gender = request.Gender,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
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

        public async Task<Result<bool>> UpdateUserAsync(Guid userId, UpdateUserDto request)
        {
            var existing = await _userRepository.GetByIdAsync(userId);
            if (existing == null)
                return Result<bool>.Fail("User to be updated doesn't exist.");

            if(!string.IsNullOrWhiteSpace(request.Username))
                existing.Name = request.Username;

            if (!string.IsNullOrWhiteSpace(request.PfpUrl))
                existing.PfpUrl = request.PfpUrl;

            if (request.Gender != existing.Gender)
                existing.Gender = request.Gender;

            existing.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(existing);
            return Result<bool>.Ok(true);
        }

        public async Task<Result<User>> GetUserByIdAsync(Guid userId)
        {
            var existing = await _userRepository.GetByIdAsync(userId);
            if (existing == null)
                return Result<User>.Fail("Failed to fetch user.");
            return Result<User>.Ok(existing);
        }
    }
}
