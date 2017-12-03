namespace P01_ProductsShop.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_ProductsShop.Data.Configurations;
    using P01_ProductsShop.Models;

    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext()
        {
        }

        public ProductsDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());

            builder.ApplyConfiguration(new ProductConfiguration());

            builder.ApplyConfiguration(new CategoryConfiguration());

            builder.ApplyConfiguration(new CategoryProductConfiguration());
        }
    }
}