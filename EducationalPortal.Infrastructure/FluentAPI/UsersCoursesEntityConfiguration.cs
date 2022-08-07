using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class UsersCoursesEntityConfiguration : IEntityTypeConfiguration<UsersCourses>
    {
        public void Configure(EntityTypeBuilder<UsersCourses> builder)
        {
            builder.HasKey(cm => new { cm.UserId, cm.CourseId });

            builder.HasOne<User>(uc => uc.User)
                   .WithMany(u => u.UsersCourses)
                   .HasForeignKey(uc => uc.UserId);

            builder.HasOne<Course>(cm => cm.Course)
               .WithMany(c => c.UsersCourses)
               .HasForeignKey(cm => cm.CourseId);
        }
    }
}
