using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class SkillsEntityConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.HasMany<CoursesSkills>(s => s.CoursesSkills)
                   .WithOne(cs => cs.Skill);

            builder.HasMany<UsersSkills>(s => s.UsersSkills)
                   .WithOne(us => us.Skill);
        }
    }
}
