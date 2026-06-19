using Microsoft.EntityFrameworkCore;
using PetFeast.Models.Orders;
using PetFeast.Models.Products;

namespace PetFeast.Data
{
    public class PetFeastDBContext : DbContext
    {
        public PetFeastDBContext(
            DbContextOptions<PetFeastDBContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
            builder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);
        }
    }
}
