using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.FinalTasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class SubmittedFinalTaskEntityConfiguration : IEntityTypeConfiguration<SubmittedFinalTask>
    {
        public void Configure(EntityTypeBuilder<SubmittedFinalTask> builder)
        {
            builder.HasOne<FinalTask>(s => s.FinalTask)
                   .WithMany(ft => ft.SubmittedFinalTasks)
                   .HasForeignKey(s => s.FinalTaskId);

            builder.HasOne<User>(s => s.User)
                   .WithMany(u => u.SubmittedFinalTasks)
                   .HasForeignKey(s => s.UserId);

            builder.HasMany<SubmittedReviewQuestion>(s => s.SubmittedReviewQuestions)
                   .WithOne(s => s.SubmittedFinalTask);

            builder.Property(s => s.ReviewedById)
                .IsRequired(false);
        } 
    }
}
