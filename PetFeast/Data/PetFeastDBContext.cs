using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetFeast.Models.Contacts;
using PetFeast.Models.Identity;
using PetFeast.Models.Orders;
using PetFeast.Models.Products;
namespace PetFeast.Data
{
    public class PetFeastDBContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);


            builder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);


            builder.Entity<Order>()
                .Property(o => o.ShippingFee)
                .HasPrecision(18, 2);


            builder.Entity<OrderDetail>()
                .Property(o => o.Price)
                .HasPrecision(18, 2);
        }
    }
}
