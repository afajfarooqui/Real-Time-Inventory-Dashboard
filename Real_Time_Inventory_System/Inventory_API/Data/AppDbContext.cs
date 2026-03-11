using Inventory_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
               

    }
}
