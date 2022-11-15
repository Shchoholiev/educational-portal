using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class UserMaterialEntityConfiguration : IEntityTypeConfiguration<UserMaterial>
    {
        public void Configure(EntityTypeBuilder<UserMaterial> builder)
        {
            builder.HasKey(um => new { um.UserId, um.MaterialId });

            builder.HasOne<User>(um => um.User)
                   .WithMany(u => u.UserMaterials)
                   .HasForeignKey(um => um.UserId);

            builder.HasOne<MaterialsBase>(um => um.Material)
                   .WithMany(m => m.UsersMaterial)
                   .HasForeignKey(um => um.MaterialId);
        }
    }
}
