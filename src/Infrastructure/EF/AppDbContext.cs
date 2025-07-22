using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    // Główny kontekst bazy danych dla Entity Framework
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Price> Prices { get; set; }
    }
}
