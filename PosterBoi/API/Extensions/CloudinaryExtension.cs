using PosterBoi.Core.Configs;

namespace PosterBoi.API.Extensions
{
    public static class CloudinaryExtension
    {
        public static IServiceCollection AddCloudinaryConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            return services;
        }
    }
}
