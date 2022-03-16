using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.FluentAPI;
using Microsoft.EntityFrameworkCore;

namespace EducationalPortal.Infrastructure.EF
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"server=(LocalDb)\MSSQLLocalDB;database=EducationalPortal;integrated security=True;
                    MultipleActiveResultSets=True;App=EntityFramework;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CoursesMaterialsEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UsersCoursesEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SkillsEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserSkillsEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialsBaseEntityConfiguration());
            modelBuilder.ApplyConfiguration(new VideoEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BookEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ShoppingHistoryEntityConfiguration());
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<UsersCourses> UsersCourses { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Quality> Qualities { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Extension> Extensions { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<ShoppingHistory> ShoppingHistory { get; set; }
    }
}
