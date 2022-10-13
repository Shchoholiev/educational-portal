using EducationalPortal.Core.Entities.FinalTasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class SubmittedReviewQuestionEntityConfiguration : IEntityTypeConfiguration<SubmittedReviewQuestion>
    {
        public void Configure(EntityTypeBuilder<SubmittedReviewQuestion> builder)
        {
            builder.HasKey(s => new { s.QuestionId, s.SubmittedFinalTaskId });

            builder.HasOne<ReviewQuestion>(s => s.Question)
                   .WithMany(q => q.SubmittedReviewQuestions)
                   .HasForeignKey(s => s.QuestionId);

            builder.HasOne<SubmittedFinalTask>(s => s.SubmittedFinalTask)
                   .WithMany(s => s.SubmittedReviewQuestions)
                   .HasForeignKey(s => s.SubmittedFinalTaskId)
                   .OnDelete(DeleteBehavior.NoAction);
        } 
    }
}
