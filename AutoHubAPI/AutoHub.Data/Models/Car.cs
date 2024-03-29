﻿namespace AutoHub.Data.Models;

public class Car : BaseEntity
{
    public string Model { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public decimal Price { get; set; }
    public int Year { get; set; }
    public string ImageUrl { get; set; } = null!;
    public virtual ICollection<Image>? Images { get; set; } = new List<Image>();
    public string Description { get; set; } = null!;

    public bool? IsSold { get; set; }

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public virtual ICollection<User>? UsersFavourite { get; set; } = new List<User>();
}