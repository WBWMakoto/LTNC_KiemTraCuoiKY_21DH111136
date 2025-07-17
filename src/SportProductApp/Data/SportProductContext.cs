using Microsoft.EntityFrameworkCore;
using SportProductApp.Models;

namespace SportProductApp.Data
{
    public class SportProductContext : DbContext
    {
        public SportProductContext(DbContextOptions<SportProductContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductID);
                entity.Property(e => e.ProductID).ValueGeneratedOnAdd();
                entity.Property(e => e.Category).HasMaxLength(20).IsFixedLength();
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ManufacturingDate).HasColumnType("date");
            });
        }
    }
}