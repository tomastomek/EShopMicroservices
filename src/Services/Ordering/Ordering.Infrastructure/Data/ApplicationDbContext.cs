using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using System.Reflection;

namespace Ordering.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrdersItems => Set<OrderItem>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // scans assembly for all classes that implement IEntityTypeConfiguration
            // and applies their configuration
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
