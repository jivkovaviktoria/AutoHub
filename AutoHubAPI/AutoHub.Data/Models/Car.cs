using System.ComponentModel.DataAnnotations.Schema;

namespace AutoHub.Data.Models;

public class Car : BaseEntity
{
    public string Model { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public decimal Price { get; set; }
    public int Year { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string Description { get; set; } = null!;

    public bool IsSold { get; set; } = false;

    public string UserId { get; set; }
    public User User { get; set; }
}