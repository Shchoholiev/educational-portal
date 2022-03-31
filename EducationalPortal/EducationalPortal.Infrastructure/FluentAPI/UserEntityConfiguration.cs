using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany<Role>(u => u.Roles)
                   .WithMany(r => r.Users);

            builder.HasMany<UsersCourses>(u => u.UsersCourses)
                   .WithOne(uc => uc.User)
                   .HasForeignKey(uc => uc.UserId);

            builder.HasMany<MaterialsBase>(u => u.Materials)
                   .WithMany(m => m.Users);

            builder.HasIndex(u => u.Email)
                   .IsUnique();
        }
    } 
}
