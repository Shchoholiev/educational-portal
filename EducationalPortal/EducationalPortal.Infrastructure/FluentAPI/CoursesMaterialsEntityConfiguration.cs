using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class CoursesMaterialsEntityConfiguration : IEntityTypeConfiguration<CoursesMaterials>
    {
        public void Configure(EntityTypeBuilder<CoursesMaterials> builder)
        {
            builder.HasKey(cm => new { cm.CourseId, cm.MaterialId });

            builder.HasOne<Course>(cm => cm.Course)
                   .WithMany(c => c.CoursesMaterials)
                   .HasForeignKey(cm => cm.CourseId);

            builder.HasOne<MaterialsBase>(cm => cm.Material)
                   .WithMany(c => c.CoursesMaterials)
                   .HasForeignKey(cm => cm.MaterialId);
        }
    }
}
