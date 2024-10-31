using EcommercePetsFoodBackend.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace EcommercePetsFoodBackend.Db_Context
{
    public class EcomContext : DbContext
    {
        public EcomContext(DbContextOptions<EcomContext> options):base(options)
        {
            
        }
        public DbSet<Customers> Customers { get; set; }
    }
}
