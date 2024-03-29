﻿using EducationalPortal.Application.FluentValidation;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.EducationalMaterials;
using EducationalPortal.Application.Interfaces.Identity;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Infrastructure.EF;
using EducationalPortal.Infrastructure.ExceptionHandling;
using EducationalPortal.Infrastructure.Repositories;
using EducationalPortal.Infrastructure.Services;
using EducationalPortal.Infrastructure.Services.EducationalMaterials;
using EducationalPortal.Infrastructure.Services.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace EducationalPortal.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SQLDatabase");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICoursesRepository, CoursesRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUsersCoursesRepository, UsersCoursesRepository>();
            services.AddScoped<ICartItemsRepository, CartItemsRepository>();
            services.AddScoped<IFinalTasksRepository, FinalTasksRepository>();
            services.AddScoped<ICertificatesRepository, CertificatesRepository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<IUsersMaterialsRepository, UsersMaterialsRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ICoursesService, CoursesService>();
            services.AddScoped<IArticlesService, ArticlesService>();
            services.AddScoped<IBooksService, BooksService>();
            services.AddScoped<IVideosService, VideosService>();
            services.AddScoped<ISkillsService, SkillsService>();
            services.AddScoped<IAuthorsService, AuthorsService>();
            services.AddScoped<IResourcesService, ResourcesService>();
            services.AddScoped<ICloudStorageService, CloudStorageService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IFinalTasksService, FinalTasksService>();
            services.AddScoped<ICertificatesService, CertificatesService>();
            services.AddScoped<IStatisticsService, StatisticsService>();

            return services;
        }

        public static IServiceCollection AddFluentValidators(this IServiceCollection services)
        {
            services.AddFluentValidation(op =>
            {
                op.LocalizationEnabled = false;
                op.DisableDataAnnotationsValidation = true;
            });
            services.AddTransient<IValidator<LoginModel>, LoginValidator>();
            services.AddTransient<IValidator<RegisterModel>, RegisterValidator>();
            services.AddTransient<IValidator<CourseCreateDto>, CourseValidator>();
            services.AddTransient<IValidator<SkillDto>, SkillValidator>();
            services.AddTransient<IValidator<BookCreateDto>, BookValidator>();
            services.AddTransient<IValidator<ArticleCreateDto>, ArticleValidator>();
            services.AddTransient<IValidator<VideoCreateDto>, VideoValidator>();
            services.AddTransient<IValidator<AuthorDto>, AuthorValidator>();
            services.AddTransient<IValidator<ResourceDto>, ResourceValidator>();

            return services;
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static ILoggingBuilder AddLogger(this ILoggingBuilder logging, IConfiguration configuration)
        {
            logging.ClearProviders();
            logging.AddNLog(configuration);

            return logging;
        }
    }
}
