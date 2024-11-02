using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Customer;
using EcommercePetsFoodBackend.Data.Models.Products;
using Microsoft.EntityFrameworkCore;
namespace EcommercePetsFoodBackend.Db_Context
{
    public class EcomContext : DbContext
    {
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
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
                    .HasOne(p => p.category)
                    .WithMany(p => p.Products);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
       
    }
}
