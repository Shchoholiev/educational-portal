using EducationalPortal.Infrastructure.EF;

namespace EducationalPortal.Infrastructure.DataInitializer
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            context.Database.EnsureDeletedAsync();
            context.Database.EnsureCreatedAsync();

            
        }
    }
}
