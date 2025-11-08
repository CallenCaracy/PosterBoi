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
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _jwtHelper = new JwtHelper(config);
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

        public async Task<Result<string>> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return Result<string>.Fail("Invalid credentials.");

            var token = _jwtHelper.GenerateJwtToken(user);
            return Result<string>.Ok(token);
        }
    }
}
