﻿using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Application.Services;
using EducationalPortal.Core.Entities;
using EducationalPortal.Infrastructure.EF;
using EducationalPortal.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EducationalPortal.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //
            var connectionString = @"server=(LocalDb)\MSSQLLocalDB;database=Store;integrated security=True;
                    MultipleActiveResultSets=True;App=EntityFramework;";

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddScoped<IGenericRepository<Course>, CoursesRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IArticlesService, ArticlesService>();
            services.AddScoped<IBooksService, BooksService>();
            services.AddScoped<IVideosService, VideosService>();
            services.AddScoped<ICoursesService, CoursesService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
