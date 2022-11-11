using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class MaterialsBaseEntityConfiguration : IEntityTypeConfiguration<MaterialsBase>
    {
        public void Configure(EntityTypeBuilder<MaterialsBase> builder)
        {
            builder.ToTable("Materials");
        }
    }
}
