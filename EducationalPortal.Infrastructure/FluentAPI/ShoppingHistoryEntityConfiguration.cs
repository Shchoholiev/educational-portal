using EducationalPortal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class ShoppingHistoryEntityConfiguration : IEntityTypeConfiguration<ShoppingHistory>
    {
        public void Configure(EntityTypeBuilder<ShoppingHistory> builder)
        {
            builder.HasOne<User>(sh => sh.User);

            builder.HasOne<Course>(sh => sh.Course);
        }
    }
}
