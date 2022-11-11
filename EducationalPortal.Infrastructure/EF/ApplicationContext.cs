using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.Courses;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.Core.Entities.FinalTasks;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CoursesMaterialsEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CoursesSkillsEntityConfiguration());
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
            modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FinalTaskEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SubmittedFinalTaskEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SubmittedReviewQuestionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserMaterialEntityConfiguration());
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<UsersCourses> UsersCourses { get; set; }

        public DbSet<CoursesMaterials> CoursesMaterials { get; set; }

        public DbSet<MaterialsBase> Materials { get; set; }

        public DbSet<CoursesSkills> CoursesSkills { get; set; }

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

        public DbSet<FinalTask> FinalTasks { get; set; }

        public DbSet<ReviewQuestion> ReviewQuestions { get; set; }

        public DbSet<SubmittedFinalTask> SubmittedFinalTasks { get; set; }

        public DbSet<SubmittedReviewQuestion> SubmittedReviewQuestions { get; set; }

        public DbSet<Certificate> Certificates { get; set; }

        public DbSet<UserMaterial> UsersMaterials { get; set; }
    }
}
