using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PosterBoi.API.Extensions;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Interfaces.Services;
using PosterBoi.Infrastructure.Data;
using PosterBoi.Infrastructure.Helpers;
using PosterBoi.Infrastructure.Repositories;
using PosterBoi.Infrastructure.Services;

namespace PosterBoi
{
    internal partial class Startup(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("PosterBoiDBConnection")));

            services.AddScoped<ICloudinaryService, CloudinaryService>();

            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostService, PostService>();

            services.AddScoped<JwtHelper>();

            services.AddCloudinaryConfiguration(_configuration);
            services.AddCorsPolicies();
        }

        public static void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
