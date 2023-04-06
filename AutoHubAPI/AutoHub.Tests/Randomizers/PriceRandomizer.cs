using TryAtSoftware.Randomizer.Core.Helpers;
using TryAtSoftware.Randomizer.Core.Interfaces;

namespace AutoHub.Tests.Randomizers;

public class PriceRandomizer : IRandomizer<decimal>
{
    private readonly decimal _price = 100.65m;
    
    public decimal PrepareRandomValue()
    {
        var randomPriceRange = RandomizationHelper.RandomInteger(0, 100000);
        return this._price + randomPriceRange;
    }
}