using EducationalPortal.API.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace EducationalPortal.API
{
    public static class MiddlewareConfigurations
    {
        public static IServiceCollection AddJWTTokenAuthentication(this IServiceCollection services, 
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

        public static IServiceCollection ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new ToKebabRouteTransformer()));
            }).AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            return services;
        }

        public static IServiceCollection ConfigureCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("allowMyOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithExposedHeaders("X-Pagination");
                });
            });

            return services;
        }
    }
}
