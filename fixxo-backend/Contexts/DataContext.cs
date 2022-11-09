using fixxo_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace fixxo_backend.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
    }
}
