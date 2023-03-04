using AutoHub.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Data;

public class AutoHubDbContext : DbContext
{
    public AutoHubDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>().HasKey(x => x.Id);
        modelBuilder.Entity<Car>().Property(x => x.Model).IsRequired();
        modelBuilder.Entity<Car>().Property(x => x.Brand).IsRequired();
        modelBuilder.Entity<Car>().Property(x => x.ImageUrl).IsRequired();
        modelBuilder.Entity<Car>().Property(x => x.Year).IsRequired();
        modelBuilder.Entity<Car>().Property(x => x.Price).IsRequired();
                
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}