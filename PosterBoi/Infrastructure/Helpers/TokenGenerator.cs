using System.Security.Cryptography;

namespace PosterBoi.Infrastructure.Helpers
{
    public static class TokenGenerator
    {
        public static string GenerateSecureToken(int length = 32)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToHexString(bytes);
        }
    }
}


