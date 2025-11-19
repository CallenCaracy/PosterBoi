using PosterBoi.Core.Configs;
using PosterBoi.Core.DTOs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Interfaces.Services;
using PosterBoi.Core.Models;
using PosterBoi.Infrastructure.Helpers;

namespace PosterBoi.Infrastructure.Services
{
    public class AuthService(IUserRepository userRepository, ISessionService sessionService) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISessionService _sessionService = sessionService;

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
                IsConfirmed = false,
                Token = TokenGenerator.GenerateSecureToken(32),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var isCreated = await _userRepository.CreateUserAsync(user);
            if (!isCreated)
                return Result<Guid>.Fail("Failed to create user.");

            return Result<Guid>.Ok(user.Id);
        }

        public async Task<Result<Jwt>> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return Result<Jwt>.Fail("Invalid credentials.");

            if (!user.IsConfirmed)
                return Result<Jwt>.Fail("The account is not confirmed please contact the admin or check the email with the confirmation for this account.");

            var tokens = await _sessionService.GenerateTokens(user);
            if (tokens == null)
                return Result<Jwt>.Fail("Failed to generate tokens.");

            return Result<Jwt>.Ok(tokens);
        }

        public async Task<Result<bool>> LogoutAsync(string refreshToken)
        {
            var isRevoked = await _sessionService.RevokeRefreshTokenAsync(refreshToken);
            if (!isRevoked)
                return Result<bool>.Fail("Failed to revoked token for logout.");

            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> UpdateUserAsync(Guid userId, UpdateUserDto request)
        {
            var existing = await _userRepository.GetByIdAsync(userId);
            if (existing == null)
                return Result<bool>.Fail("User to be updated doesn't exist.");

            existing.Name = request.Username;
            existing.PfpUrl = request.PfpUrl;
            existing.Gender = request.Gender;
            existing.Birthday = request.Birthday;
            existing.UpdatedAt = DateTime.UtcNow;

            var isUpdated = await _userRepository.UpdateUserAsync(existing);
            if (!isUpdated)
                return Result<bool>.Fail("Failed to update user.");

            return Result<bool>.Ok(true);
        }

        public async Task<Result<User>> GetUserByIdAsync(Guid userId)
        {
            var existing = await _userRepository.GetByIdAsync(userId);
            if (existing == null)
                return Result<User>.Fail("Failed to fetch user.");
            return Result<User>.Ok(existing);
        }

        public async Task<Result<bool>> ConfirmUserAsync(string token)
        {
            var existing = await _userRepository.GetByTokenAsync(token);
            if (existing == null)
                return Result<bool>.Fail("Failed to find the user to confirm.");

            if (existing.IsConfirmed)
                return Result<bool>.Fail("The user is already been confirmed.");

            existing.IsConfirmed = true;
            existing.Token = null;

            var isConfirmed = await _userRepository.UpdateIsConfirmAsync(existing);
            if (!isConfirmed)
                return Result<bool>.Fail("Failed to confirm user account.");

            return Result<bool>.Ok(isConfirmed);
        }
    }
}
