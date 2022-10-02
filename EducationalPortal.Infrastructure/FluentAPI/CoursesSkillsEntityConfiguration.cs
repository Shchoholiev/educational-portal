using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class CoursesSkillsEntityConfiguration : IEntityTypeConfiguration<CoursesSkills>
    {
        public void Configure(EntityTypeBuilder<CoursesSkills> builder)
        {
            builder.HasKey(cm => new { cm.CourseId, cm.SkillId });

            builder.HasOne<Course>(cm => cm.Course)
                   .WithMany(c => c.CoursesSkills)
                   .HasForeignKey(cm => cm.CourseId);

            builder.HasOne<Skill>(cm => cm.Skill)
                   .WithMany(c => c.CoursesSkills)
                   .HasForeignKey(cm => cm.SkillId);
        }
    }
}
