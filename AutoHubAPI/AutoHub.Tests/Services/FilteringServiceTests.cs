using AutoHub.Core.Contracts;
using AutoHub.Core.FilterDefinitions;
using AutoHub.Core.Services;
using AutoHub.Data.Models;
using AutoHub.Tests.Randomizers;
using AutoHub.Utilities;
using Moq;
using TryAtSoftware.Equalizer.Core;
using TryAtSoftware.Equalizer.Core.ProfileProviders;
using TryAtSoftware.Equalizer.Core.Profiles.General;

namespace AutoHub.Tests.Services;

public class FilteringServiceTests
{
    private readonly Equalizer _equalizer;
    private readonly Mock<IService<Car>> _mockService;
    private readonly CarRandomizer _carRandomizer;

    public FilteringServiceTests()
    {
        var profileProvider = new DedicatedProfileProvider();
        profileProvider.AddProfile(new GeneralEqualizationProfile<Car>());
        
        this._equalizer = new Equalizer();
        this._equalizer.AddProfileProvider(profileProvider);

        this._carRandomizer = new CarRandomizer();
        this._mockService = new Mock<IService<Car>>();
    }
    
    [Fact]
    public async void OrderByShouldReturnCorrectlyOrderedCollectionAscending()
    {
        var orderDefinition = new OrderDefinition() { Property = "Price", IsAscending = true };

        List<Car> cars = new();
        for (int i = 0; i < 5; i++) cars.Add(this._carRandomizer.PrepareRandomValue());
        
        var expected = cars.OrderBy(x => x.Price);

        this._mockService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = cars });

        var filteringService = new FilteringService(this._mockService.Object);

        var result = await filteringService.OrderBy(orderDefinition);
        
        this._equalizer.AssertEquality(true, result.IsSuccessful);
        this._equalizer.AssertEquality(expected, result.Data);
    }

    [Fact]
    public async void OrderByShouldReturnCorrectlyOrderedCollectionDescending()
    {
        var orderDefinition = new OrderDefinition() { Property = "Price", IsAscending = false };

        List<Car> cars = new();
        for (int i = 0; i < 5; i++) cars.Add(this._carRandomizer.PrepareRandomValue());
        
        var expected = cars.OrderByDescending(x => x.Price);

        this._mockService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = cars });

        var filteringService = new FilteringService(this._mockService.Object);

        var result = await filteringService.OrderBy(orderDefinition);
        
        this._equalizer.AssertEquality(true, result.IsSuccessful);
        this._equalizer.AssertEquality(expected, result.Data);
    }

    [Fact]
    public async void FilterByPriceShouldReturnsFilteredCollection()
    {
        var filterDefinition = new PriceFilterDefinition() { Min = 2000, Max = 10000 };
        var globalFilter = new GlobalCarFilter() { Price = filterDefinition };
        
        List<Car> cars = new();
        for(int i = 0; i < 5; i++) cars.Add(this._carRandomizer.PrepareRandomValue());

        var expected = cars.Where(x => x.Price >= filterDefinition.Min && x.Price <= filterDefinition.Max);

        this._mockService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = cars });

        var filteringService = new FilteringService(this._mockService.Object);
        
        var result = await filteringService.Filter(cars, globalFilter);
        
        this._equalizer.AssertEquality(true, result.IsSuccessful);
        this._equalizer.AssertEquality(expected, result.Data);
    }

    [Fact]
    public async void FilterWithMultipleFilterDefinitionsShouldWorkCorrectly()
    {
        var priceFilter = new PriceFilterDefinition() { Min = 1, Max = 10000 };
        var yearFilter = new YearFilterDefinition() { Min = 2000, Max = 2023 };

        var globalFilter = new GlobalCarFilter()
        {
            Price = priceFilter, Year = yearFilter, Models = new List<string>{ "Audi" },
            Brands = new List<string>() { "Rs7" }
        };

        List<Car> cars = new();
        for(int i = 0; i < 10; i++) cars.Add(this._carRandomizer.PrepareRandomValue());
        
        cars.Add(new Car(){Price = 1000, Year = 2010, Brand = "rs7", Model = "audi"});
        
        this._mockService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = cars });
        
        var filteringService = new FilteringService(this._mockService.Object);
        
        var result = await filteringService.Filter(cars, globalFilter);
        
        var expected = cars.Where(x =>
            x.Price >= priceFilter.Min && x.Price <= priceFilter.Max && x.Year >= yearFilter.Min &&
            x.Year <= yearFilter.Max && globalFilter.Models.Contains(x.Model, StringComparer.OrdinalIgnoreCase) &&
            globalFilter.Brands.Contains(x.Brand, StringComparer.OrdinalIgnoreCase));
        
        this._equalizer.AssertEquality(true, result.IsSuccessful);
        this._equalizer.AssertEquality(expected, result.Data);
    }

    [Fact]
    public async void FilterWithNullGlobalFilterShouldWorkCorectly()
    {
        List<Car> cars = new();
        for(int i = 0; i < 10; i++) cars.Add(this._carRandomizer.PrepareRandomValue());
        
        this._mockService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = cars });
        
        var filteringService = new FilteringService(this._mockService.Object);

        var result = await filteringService.Filter(cars, null);
        
        this._equalizer.AssertEquality(true, result.IsSuccessful);
        this._equalizer.AssertEquality(cars, result.Data);
    }
}