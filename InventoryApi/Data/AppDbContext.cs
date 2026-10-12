using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(e =>
            {
                e.Property(p => p.Name).IsRequired().HasMaxLength(200);
                e.Property(p => p.Sku).IsRequired().HasMaxLength(50);
                e.HasIndex(p => p.Sku).IsUnique();
                e.Property(p => p.Price).HasPrecision(18, 2);
            });

            modelBuilder.Entity<OrderItem>(e =>
            {
                e.Property(i => i.UnitPrice).HasPrecision(18, 2);
                e.HasOne(i => i.Product)
                 .WithMany()
                 .HasForeignKey(i => i.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
