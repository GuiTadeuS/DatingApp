using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions // Static == use methods without instantiating
    {
        // targeting the type in the signature with the "this" keyword
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.GetConnectionString("Redis");
            });

            services.AddCors();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddAutoMapper(typeof(AutoMapperProfiles));

            return services;
        }
    }
}
