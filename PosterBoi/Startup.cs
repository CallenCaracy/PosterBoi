using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosterBoi.API.Extensions;
using PosterBoi.Core.Interfaces;
using PosterBoi.Infrastructure.Data;
using PosterBoi.Infrastructure.Services;

namespace PosterBoi
{
    internal partial class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
            services.AddSwaggerGen();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("PosterBoiDBConnection")));

            services.AddScoped<ICloudinaryService, CloudinaryService>();

            services.AddCloudinaryConfiguration(_configuration);
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowedFrontend");
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
