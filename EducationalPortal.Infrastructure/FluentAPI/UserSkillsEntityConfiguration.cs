using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class UserSkillsEntityConfiguration : IEntityTypeConfiguration<UsersSkills>
    {
        public void Configure(EntityTypeBuilder<UsersSkills> builder)
        {
            builder.HasKey(cm => new { cm.UserId, cm.SkillId });

            builder.HasOne<User>(us => us.User)
                   .WithMany(u => u.UsersSkills)
                   .HasForeignKey(us => us.UserId);

            builder.HasOne<Skill>(us => us.Skill)
                   .WithMany(s => s.UsersSkills)
                   .HasForeignKey(us => us.SkillId);
        }
    }
}
