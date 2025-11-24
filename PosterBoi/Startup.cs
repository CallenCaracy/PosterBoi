using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosterBoi.API.Extensions;
using PosterBoi.Core.Configs;
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
                c.AddSecurityDefinition(Constanst.BearerScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
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

            // Database
            var connectionString = _configuration["POSTERBOI_DB_CONNECTION"];
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("POSTERBOI_DB_CONNECTION environment variable is missing!");
            }
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Email
            services.Configure<EmailSettings>(options =>
            {
                options.Host = _configuration["EMAIL_HOST"]!;
                options.Port = int.Parse(_configuration["EMAIL_PORT"] ?? "587");
                options.Username = _configuration["EMAIL_USERNAME"]!;
                options.Password = _configuration["EMAIL_PASSWORD"]!;
                options.From = _configuration["EMAIL_FROM"]!;
            });
            services.AddTransient<IEmailService, EmailService>();

            // Cloudinary
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            // Auth
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Post
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostRepository, PostRepository>();

            // Reactions
            services.AddScoped<IReactionService, ReactionService>();
            services.AddScoped<IReactionRepository, ReactionRepository>();

            // Comments
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            // Helpers
            services.AddScoped<JwtHelper>();

            services.AddCloudinaryConfiguration(_configuration);
            services.AddCorsPolicies();
        }

        public static void Configure(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
