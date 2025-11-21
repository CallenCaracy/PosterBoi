using PosterBoi.Core.Configs;

namespace PosterBoi.API.Extensions
{
    public static class CloudinaryExtension
    {
        public static IServiceCollection AddCloudinaryConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CloudinarySettings>(options =>
            {
                options.CloudName = configuration["CLOUDINARY_CLOUDNAME"]!;
                options.ApiKey = configuration["CLOUDINARY_APIKEY"]!;
                options.ApiSecret = configuration["CLOUDINARY_APISECRET"]!;
            });

            return services;
        }
    }
}
