using EducationalPortal.Application.DTO;
using EducationalPortal.API.FluentValidation;
using EducationalPortal.API.ViewModels;
using FluentValidation;
using FluentValidation.AspNetCore;
using EducationalPortal.Application.DTO.EducationalMaterials;
using EducationalPortal.Application.DTO.EducationalMaterials.Properties;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        public static IServiceCollection AddJWTTokenServices(this IServiceCollection services, 
                                                             IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateIssuer"),
                    ValidateAudience = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateAudience"),
                    ValidateLifetime = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateLifetime"),
                    ValidateIssuerSigningKey = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateIssuerSigningKey"),
                    ValidIssuer = configuration.GetValue<string>("JsonWebTokenKeys:ValidIssuer"),
                    ValidAudience = configuration.GetValue<string>("JsonWebTokenKeys:ValidAudience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        configuration.GetValue<string>("JsonWebTokenKeys:IssuerSigningKey")))
                };
            });

            return services;
        }
    }
}
