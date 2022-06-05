using EducationalPortal.API.FluentValidation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Models.CreateDTO;

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
