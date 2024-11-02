using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // id is CustomerId, we need to use conversion to tell EF Core
            // how to store it in database
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasConversion(
                // we use customerId.Value when storing information in database
                customerId => customerId.Value,
                // when reading from database we use CustomerId.Of(dbId) to reach
                // customerId in our application
                dbId => CustomerId.Of(dbId));

            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Email).HasMaxLength(255).IsRequired();
            // configures an unnamed index
            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
