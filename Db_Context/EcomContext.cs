using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace EcommercePetsFoodBackend.Db_Context
{
    public class EcomContext : DbContext
    {
        public DbSet<Customers> Customers { get; set; }
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

        
        }
    }
}
