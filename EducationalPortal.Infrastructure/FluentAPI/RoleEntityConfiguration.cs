using EducationalPortal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasMany<User>(r => r.Users)
                   .WithMany(u => u.Roles);

            builder.HasIndex(r => r.Name)
                   .IsUnique();
        }
    }
}
