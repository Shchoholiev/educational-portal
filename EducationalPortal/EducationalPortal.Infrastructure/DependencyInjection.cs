﻿using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.EducationalMaterials;
using EducationalPortal.Application.Interfaces.Identity;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Infrastructure.EF;
using EducationalPortal.Infrastructure.IRepositories;
using EducationalPortal.Infrastructure.Services;
using EducationalPortal.Infrastructure.Services.EducationalMaterials;
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
            services.AddScoped<IUsersCoursesRepository, UsersCoursesRepository>();
            services.AddScoped<ICartItemsRepository, ICartItemsRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IArticlesService, ArticlesService>();
            services.AddScoped<IBooksService, BooksService>();
            services.AddScoped<IVideosService, VideosService>();
            services.AddScoped<ISkillsService, SkillsService>();
            services.AddScoped<IAuthorsService, AuthorsService>();
            services.AddScoped<IResourcesService, ResourcesService>();
            services.AddScoped<ICloudStorageService, CloudStorageService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            return services;
        }
    }
}
