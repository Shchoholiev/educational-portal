using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.FinalTasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class FinalTaskEntityConfiguration : IEntityTypeConfiguration<FinalTask>
    {
        public void Configure(EntityTypeBuilder<FinalTask> builder)
        {
            builder.HasMany<ReviewQuestion>(ft => ft.ReviewQuestions)
                   .WithOne(rq => rq.FinalTask);

            builder.HasMany<Course>(ft => ft.Courses)
                   .WithOne(c => c.FinalTask)
                   .IsRequired(false);

            builder.HasIndex(ft => ft.Name)
                   .IsUnique();
        } 
    }
}
