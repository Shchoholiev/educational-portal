using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasMany<CoursesSkills>(c => c.CoursesSkills)
                   .WithOne(cs => cs.Course);

            builder.HasMany<CoursesMaterials>(c => c.CoursesMaterials)
                   .WithOne(cm => cm.Course);

            builder.HasMany<UsersCourses>(c => c.UsersCourses)
                   .WithOne(uc => uc.Course);

            builder.Ignore(c => c.Materials);

            builder.Ignore(c => c.Skills);
        } 
    }
}
