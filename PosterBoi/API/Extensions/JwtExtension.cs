using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PosterBoi.API.Extensions
{
    public static class JwtExtension
    {
        public static IServiceCollection AddAuthenticationJwt(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JWT_ISSUER"],
                        ValidAudience = builder.Configuration["JWT_AUDIENCE"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JWT_KEY"]!)
                        )
                    };
                });
            return services;
        }
    }
}
