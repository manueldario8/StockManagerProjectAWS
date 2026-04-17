using StockManager.API.Entities.Models.Catalog;
using Microsoft.EntityFrameworkCore;

namespace StockManager.API.Data
{
    public class DataBaseContext(DbContextOptions<DataBaseContext> options) : DbContext(options)
    {
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Providers
            modelBuilder.Entity<Provider>()
                .Property(p => p.Code);
            modelBuilder.Entity<Provider>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<Provider>()
            .Property(c => c.StatusActived)
            .HasDefaultValue(true);

            modelBuilder.Entity<Provider>()
            .HasQueryFilter(c => c.StatusActived);

            //Categories
            modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

            modelBuilder.Entity<Category>()
            .Property(c => c.StatusActived)
            .HasDefaultValue(true);

            modelBuilder.Entity<Category>()
            .HasQueryFilter(c => c.StatusActived);

            // Products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Provider)
                .WithMany(p => p.Products)
                .HasPrincipalKey(b => b.Code)
                .HasForeignKey(p => p.ProviderCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.ProviderCode, p.ProductCode })
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .UsingEntity(j => j.ToTable("ProductCategories"));

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
            .Property(c => c.StatusActived)
            .HasDefaultValue(true);

            modelBuilder.Entity<Product>()
            .HasQueryFilter(c => c.StatusActived);

        }
    }
}
