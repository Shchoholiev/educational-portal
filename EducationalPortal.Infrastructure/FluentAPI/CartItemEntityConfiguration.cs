using EducationalPortal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationalPortal.Infrastructure.FluentAPI
{
    public class CartItemEntityConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasOne<Course>(ci => ci.Course);

            builder.HasOne<User>(ci => ci.User);
        }
    }
}
