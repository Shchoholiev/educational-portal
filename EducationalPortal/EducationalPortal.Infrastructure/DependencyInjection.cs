using EducationalPortal.Application.Interfaces;
using EducationalPortal.Infrastructure.EF;
using EducationalPortal.Infrastructure.IRepositories;
using EducationalPortal.Infrastructure.Services;
using EducationalPortal.Infrastructure.Services.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EducationalPortal.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SQLDataBase");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddScoped<ICoursesRepository, CoursesRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<ICloudStorageService, CloudStorageService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            return services;
        }
    }
}
