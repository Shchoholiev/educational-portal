using EducationalPortal.Application.DTO;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.API.FluentValidation;
using EducationalPortal.API.ViewModels;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace EducationalPortal.API.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFluentValidators(this IServiceCollection services)
        {
            services.AddFluentValidation(op =>
            {
                op.LocalizationEnabled = false;
                op.DisableDataAnnotationsValidation = true;
            });
            services.AddTransient<IValidator<LoginViewModel>, LoginValidator>();
            services.AddTransient<IValidator<RegisterViewModel>, RegisterValidator>();
            services.AddTransient<IValidator<CourseDTO>, CourseValidator>();
            services.AddTransient<IValidator<Skill>, SkillValidator>();
            services.AddTransient<IValidator<BookDTO>, BookValidator>();
            services.AddTransient<IValidator<ArticleDTO>, ArticleValidator>();
            services.AddTransient<IValidator<VideoDTO>, VideoValidator>();
            services.AddTransient<IValidator<Author>, AuthorValidator>();
            services.AddTransient<IValidator<Resource>, ResourceValidator>();

            return services;
        }
    }
}
