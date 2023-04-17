using AutoHub.Core.FilterDefinitions.Definitions;
using AutoHub.Data.Models;

namespace AutoHub.Core.FilterDefinitions;

public class GlobalCarFilter
{
    public PriceFilterDefinition? Price { get; set; }
    public YearFilterDefinition? Year { get; set; }
    public IEnumerable<string>? Models { get; set; }
    public IEnumerable<string>? Brands { get; set; }

    public IEnumerable<Car> Filter(IEnumerable<Car> cars)
    {
        if (this.Price != null) cars = cars.Where(x => x.Price >= this.Price.Min && x.Price <= this.Price.Max);
        if (this.Year != null) cars = cars.Where(x => x.Year >= this.Year.Min && x.Year <= this.Year.Max);
        if (this.Models != null) cars = cars.Where(x => this.Models.Contains(x.Model, StringComparer.OrdinalIgnoreCase));
        if (this.Brands != null) cars = cars.Where(x => this.Brands.Contains(x.Brand, StringComparer.OrdinalIgnoreCase));

        return cars;
    }
}