using EducationalPortal.Application.DTO;
using EducationalPortal.API.FluentValidation;
using EducationalPortal.API.ViewModels;
using FluentValidation;
using FluentValidation.AspNetCore;
using EducationalPortal.Application.DTO.EducationalMaterials;
using EducationalPortal.Application.DTO.EducationalMaterials.Properties;

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
            services.AddTransient<IValidator<SkillDTO>, SkillValidator>();
            services.AddTransient<IValidator<BookDTO>, BookValidator>();
            services.AddTransient<IValidator<ArticleDTO>, ArticleValidator>();
            services.AddTransient<IValidator<VideoDTO>, VideoValidator>();
            services.AddTransient<IValidator<AuthorDTO>, AuthorValidator>();
            services.AddTransient<IValidator<ResourceDTO>, ResourceValidator>();

            return services;
        }
    }
}
