namespace P03_SalesDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using P03_SalesDatabase.Data.Models;

    public class SalesContext : DbContext
    {
        public SalesContext()
        {

        }

        public SalesContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>(en =>
            {
                en.Property(p => p.Name).IsRequired().HasMaxLength(50).IsUnicode();
                en.Property(p => p.Quantity).IsRequired();
                en.Property(p => p.Price).IsRequired();
                en.Property(p => p.Description).HasMaxLength(250).IsUnicode().HasDefaultValue("No description");
            });

            builder.Entity<Customer>(en =>
            {
                en.Property(c => c.Name).IsRequired().HasMaxLength(100).IsUnicode();
                en.Property(c => c.Email).HasColumnType("varchar(80)");
                en.Property(c => c.CreditCardNumber).IsRequired();
            });

            builder.Entity<Store>(en =>
            {
                en.Property(s => s.Name).IsRequired().HasMaxLength(80).IsUnicode();
            });

            builder.Entity<Sale>(en =>
            {
                en.Property(s => s.Date).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            builder.Entity<Product>()
                .HasMany(p => p.Sales)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId);

            builder.Entity<Customer>()
                .HasMany(c => c.Sales)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerId);

            builder.Entity<Store>()
                .HasMany(st => st.Sales)
                .WithOne(sale => sale.Store)
                .HasForeignKey(sale => sale.StoreId);

            builder.Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.ProductId);

            builder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId);

            builder.Entity<Sale>()
                .HasOne(sale => sale.Store)
                .WithMany(st => st.Sales)
                .HasForeignKey(sale => sale.StoreId);
        }
    }
}