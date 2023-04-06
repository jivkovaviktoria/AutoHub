namespace AutoHub.Data.ViewModels;

public class CarInfoViewModel
{
    public Guid Id { get; set; }
    public string Model { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public decimal Price { get; set; }
    public int Year { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string Description { get; set; } = null!;

    public List<string> Images { get; set; }
}