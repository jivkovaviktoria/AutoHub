using AutoHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Data;

public class AutoHubDbContext : IdentityUserContext<User>
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

        modelBuilder.Entity<Car>().HasOne(c => c.User)
            .WithMany(u => u.Cars)
            .HasForeignKey(c => c.UserId);
        
        base.OnModelCreating(modelBuilder);
    }
}