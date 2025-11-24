namespace PosterBoi.API.Extensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowedFrontend", builder =>
                {
                    builder
                        .WithOrigins(
                            "http://localhost:5173",
                            "http://localhost:3000"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            return services;
        }
    }
}
