using EducationalPortal.Application.DTO;
using EducationalPortal.Core.Entities;
using EducationalPortal.Web.FluentValidation;
using EducationalPortal.Web.ViewModels;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace EducationalPortal.Web.DI
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

            return services;
        }
    }
}
