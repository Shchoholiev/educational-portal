using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EducationalPortal.Infrastructure.CustomMiddlewares
{
    public class AppHttpContext
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static HttpContext Current => _httpContextAccessor.HttpContext;

        public static string BaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
        }
    }

    public static class HttpContextExtensions
    {
        public static void AddAppHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseAppHttpContext(this IApplicationBuilder app)
        {
            AppHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }
    }
}
