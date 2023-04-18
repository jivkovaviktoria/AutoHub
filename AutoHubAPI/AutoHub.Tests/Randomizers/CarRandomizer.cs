using AutoHub.Data.Models;
using TryAtSoftware.Randomizer.Core;
using TryAtSoftware.Randomizer.Core.Primitives;

namespace AutoHub.Tests.Randomizers;

public class CarRandomizer : ComplexRandomizer<Car>
{
    public CarRandomizer()
    {
        this.AddRandomizationRule(c => c.Id, new GuidRandomizer());
        this.AddRandomizationRule(c => c.Model, new StringRandomizer());
        this.AddRandomizationRule(c => c.Brand, new StringRandomizer());
        this.AddRandomizationRule(c => c.Price, new PriceRandomizer());
        this.AddRandomizationRule(c => c.Description, new StringRandomizer());
        this.AddRandomizationRule(c => c.Year, new NumberRandomizer());
        this.AddRandomizationRule(c => c.ImageUrl, new StringRandomizer());
    }
}