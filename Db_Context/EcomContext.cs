using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.cartmodel;
using EcommercePetsFoodBackend.Data.Models.Customer;
using EcommercePetsFoodBackend.Data.Models.Products;
using EcommercePetsFoodBackend.Data.Models.Wishlists;
using EcommercePetsFoodBackend.Services.CustomerServices;
using Microsoft.EntityFrameworkCore;
namespace EcommercePetsFoodBackend.Db_Context
{
    public class EcomContext : DbContext
    {
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public EcomContext(DbContextOptions<EcomContext> options):base(options)
        {
            
         }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

       

            var AdminPassword= BCrypt.Net.BCrypt.HashPassword("admin@123");
            modelBuilder.Entity<Customers>().HasData(
                new Customers
                {
                    Id = 1, 
                    Name = "Admin User",
                    Email = "admin@example.com",
                    Password = AdminPassword,
                    Role = "admin",
                    IsBlocked = false 
                });

            modelBuilder.Entity<Product>()
                    .HasOne(p => p.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(p => p.ProductCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Wishlist>()
                .HasOne(u => u.customer)
                .WithMany(w => w.Wishlist)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<Wishlist>()
                .HasOne(p => p.product)
                .WithMany(w => w.wishlists)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Customers>()
                .HasOne(ca => ca.cart)
                .WithOne(c => c.customer)
                .HasForeignKey<Cart>(c => c.UserId);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(c => c.Cart)
                .HasForeignKey(c => c.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(p=>p.Product)
                .WithMany(c => c.Items)
                .HasForeignKey(c => c.ProductId);

                }
       
    }
}
