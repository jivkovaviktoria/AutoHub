using AutoHub.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Data;

public class AutoHubDbContext : IdentityUserContext<User>
{
    public AutoHubDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Car> Cars { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Car>().HasKey(x => x.Id);
        builder.Entity<Car>().Property(x => x.Model).IsRequired();
        builder.Entity<Car>().Property(x => x.Brand).IsRequired();
        builder.Entity<Car>().Property(x => x.ImageUrl).IsRequired();
        builder.Entity<Car>().Property(x => x.Year).IsRequired();
        builder.Entity<Car>().Property(x => x.Price).IsRequired();

        builder.Entity<Car>().HasOne(c => c.User)
            .WithMany(u => u.Cars)
            .HasForeignKey(c => c.UserId);

        builder.Entity<Car>().HasMany(c => c.UsersFavourite)
            .WithMany(u => u.FavouriteCars);

        builder.Entity<Image>().HasOne(i => i.Car).WithMany(c => c.Images).HasForeignKey(i => i.CarId);

        builder.Entity<Review>().HasOne(x => x.Owner).WithMany(x => x.WrittenReviews);
        builder.Entity<Review>().HasOne(x => x.User).WithMany(x => x.ReceivedReviews);
        
        base.OnModelCreating(builder);
    }
}