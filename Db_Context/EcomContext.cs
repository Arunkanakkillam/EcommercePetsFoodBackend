using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace EcommercePetsFoodBackend.Db_Context
{
    public class EcomContext : DbContext
    {
        public EcomContext(DbContextOptions<EcomContext> options):base(options)
        {



         }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LoginDto>().HasNoKey();

            modelBuilder.Ignore<LoginDto>();
        }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<LoginDto> LoginDto { get; set; }
    }
}
