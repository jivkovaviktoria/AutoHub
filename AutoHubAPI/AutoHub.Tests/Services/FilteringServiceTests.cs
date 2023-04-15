using AutoHub.Core.Contracts;
using AutoHub.Core.FilterDefinitions;
using AutoHub.Core.Services;
using AutoHub.Data.Models;
using AutoHub.Tests.Randomizers;
using AutoHub.Utilities;
using Moq;

namespace AutoHub.Tests.Services;

public class FilteringServiceTests
{
    [Fact]
    public async void OrderByShouldReturnCorrectlyOrderedCollectionAscending()
    {
        var orderDefinition = new OrderDefinition() { Property = "Price", IsAscending = true };

        List<Car> cars = new();

        var carRandomizer = new CarRandomizer();
        for (int i = 0; i < 5; i++) cars.Add(carRandomizer.PrepareRandomValue());
        
        var expected = cars.OrderBy(x => x.Price);

        var mockService = new Mock<IService<Car>>();
        mockService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = cars });

        var filteringService = new FilteringService(mockService.Object);

        var result = await filteringService.OrderBy(orderDefinition);
        
        Assert.True(result.IsSuccessful);
        Assert.Equal(expected, result.Data);
    }

    [Fact]
    public async void OrderByShouldReturnCorrectlyOrderedCollectionDescending()
    {
        var orderDefinition = new OrderDefinition() { Property = "Price", IsAscending = false };

        List<Car> cars = new();

        var carRandomizer = new CarRandomizer();
        for (int i = 0; i < 5; i++) cars.Add(carRandomizer.PrepareRandomValue());
        
        var expected = cars.OrderByDescending(x => x.Price);

        var mockService = new Mock<IService<Car>>();
        mockService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = cars });

        var filteringService = new FilteringService(mockService.Object);

        var result = await filteringService.OrderBy(orderDefinition);
        
        Assert.True(result.IsSuccessful);
        Assert.Equal(expected, result.Data);
    }

    [Fact]
    public async void FilterByPriceShouldReturnsFilteredCollection()
    {
        var filterDefinition = new PriceFilterDefinition() { Min = 2000, Max = 10000 };
        var globalFilter = new GlobalCarFilter() { Price = filterDefinition };
        
        List<Car> cars = new();

        var carRandomizer = new CarRandomizer();
        for(int i = 0; i < 5; i++) cars.Add(carRandomizer.PrepareRandomValue());

        var expected = cars.Where(x => x.Price >= filterDefinition.Min && x.Price <= filterDefinition.Max);

        var mockService = new Mock<IService<Car>>();
        mockService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = cars });

        var filteringService = new FilteringService(mockService.Object);
        
        var result = await filteringService.Filter(cars, globalFilter);
        
        Assert.True(result.IsSuccessful);
        Assert.Equal(expected, result.Data);
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
        
        var carRandomizer = new CarRandomizer();
        for(int i = 0; i < 10; i++) cars.Add(carRandomizer.PrepareRandomValue());
        
        cars.Add(new Car(){Price = 1000, Year = 2010, Brand = "rs7", Model = "audi"});
        
        var mockService = new Mock<IService<Car>>();
        mockService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = cars });
        
        var filteringService = new FilteringService(mockService.Object);
        
        var result = await filteringService.Filter(cars, globalFilter);
        
        var expected = cars.Where(x =>
            x.Price >= priceFilter.Min && x.Price <= priceFilter.Max && x.Year >= yearFilter.Min &&
            x.Year <= yearFilter.Max && globalFilter.Models.Contains(x.Model, StringComparer.OrdinalIgnoreCase) &&
            globalFilter.Brands.Contains(x.Brand, StringComparer.OrdinalIgnoreCase));
        
        Assert.True(result.IsSuccessful);
        Assert.Equal(expected, result.Data);
    }

    [Fact]
    public async void FilterWithNullGlobalFilterShouldWorkCorectly()
    {
        List<Car> cars = new();
        
        var carRandomizer = new CarRandomizer();
        for(int i = 0; i < 10; i++) cars.Add(carRandomizer.PrepareRandomValue());
        
        var mockService = new Mock<IService<Car>>();
        mockService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = cars });
        
        var filteringService = new FilteringService(mockService.Object);

        var result = await filteringService.Filter(cars, null);
        
        Assert.True(result.IsSuccessful);
        Assert.Equal(cars, result.Data);
    }
}