using API.Data;
using API.Interfaces;
using API.Repository;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
     public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt => {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // AddAutoMapper is in Extensions folder that contain AutoMapperExtensions.cs file with AddAutoMapper and GetAssemblies to current domain
            return services;
        }
    }
}
