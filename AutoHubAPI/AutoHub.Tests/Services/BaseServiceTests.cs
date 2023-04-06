using AutoHub.Core.Services;
using AutoHub.Data.Contracts;
using AutoHub.Data.Models;
using AutoHub.Tests.Randomizers;
using AutoHub.Utilities;
using Moq;
using TryAtSoftware.Randomizer.Core.Primitives;

namespace AutoHub.Tests.Services;

public class BaseServiceTests
{
    [Fact]
    public async void GetByWithValidIdReturnsEntity()
    {
        var carRandomizer = new CarRandomizer();
        var expectedEntity = carRandomizer.PrepareRandomValue();

        var mockRepository = new Mock<IRepository<Car>>();
        mockRepository.Setup(r => r.GetAsync(expectedEntity.Id))
            .ReturnsAsync(new OperationResult<Car>(){Data = expectedEntity});

        var service = new Service<Car>(mockRepository.Object);

        var result = await service.GetAsync(expectedEntity.Id);
        
        Assert.True(result.IsSuccessful);
        Assert.Equal(expectedEntity, result.Data);
    }

    [Fact]
    public async void GetByIdWithInvalidIdShouldNotReturnEntity()
    {
        var guidRandomizer = new GuidRandomizer();
        var entityId = guidRandomizer.PrepareRandomValue();
        
        var mockRepository = new Mock<IRepository<Car>>();
        mockRepository.Setup(r => r.GetAsync(entityId))
            .ReturnsAsync(new OperationResult<Car>(){Data = null});

        var service = new Service<Car>(mockRepository.Object);

        var result = await service.GetAsync(entityId);
        
        Assert.True(result.IsSuccessful);
        Assert.Null(result.Data);
    }

    [Fact]
    public async void GetManyShouldReturnCollectionOfEntities()
    {
        var carRandomizer = new CarRandomizer();
        
        List<Car> cars = new List<Car>();
        for(int i = 0; i < 5; i++) cars.Add(carRandomizer.PrepareRandomValue());
        
        var mockRepository = new Mock<IRepository<Car>>();
        mockRepository.Setup(r => r.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>(){Data = cars});

        var service = new Service<Car>(mockRepository.Object);

        var result = await service.GetManyAsync();
        
        Assert.True(result.IsSuccessful);
        Assert.Equal(cars, result.Data);
    }

    [Fact]
    public async void GetManyShouldReturnEmptyCollection()
    {
        var mockRepository = new Mock<IRepository<Car>>();
        mockRepository.Setup(r => r.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>(){Data = Enumerable.Empty<Car>()});

        var service = new Service<Car>(mockRepository.Object);

        var result = await service.GetManyAsync();
        
        Assert.True(result.IsSuccessful);
        Assert.Equal(Enumerable.Empty<Car>(), result.Data);
    }

    [Fact]
    public async void AnyAsyncShouldReturnTrue()
    {
        var carRandomizer = new CarRandomizer();
        var entity = carRandomizer.PrepareRandomValue();

        var mockRepository = new Mock<IRepository<Car>>();
        mockRepository.Setup(r => r.AnyAsync(entity.Id))
            .ReturnsAsync(new OperationResult<bool>(){Data = true});

        var service = new Service<Car>(mockRepository.Object);

        var result = await service.AnyAsync(entity.Id);
        
        Assert.True(result.IsSuccessful);
        Assert.True(result.Data);
    }

    [Fact]
    public async void AnyAsyncShouldReturnFalse()
    {
        var guidRandomizer = new GuidRandomizer();
        var entityId = guidRandomizer.PrepareRandomValue();
        
        var mockRepository = new Mock<IRepository<Car>>();
        mockRepository.Setup(r => r.AnyAsync(entityId))
            .ReturnsAsync(new OperationResult<bool>() { Data = false });

        var service = new Service<Car>(mockRepository.Object);

        var result = await service.AnyAsync(entityId);
        
        Assert.True(result.IsSuccessful);
        Assert.False(result.Data);
    }

    [Fact]
    public async void CreateShouldReturnEntity()
    {
        var carRandomizer = new CarRandomizer();
        var entity = carRandomizer.PrepareRandomValue();

        var mockRepository = new Mock<IRepository<Car>>();
        mockRepository.Setup(r => r.CreateAsync(entity))
            .ReturnsAsync(new OperationResult<Car>() { Data = entity });

        var service = new Service<Car>(mockRepository.Object);

        var result = await service.CreateAsync(entity);
        
        Assert.True(result.IsSuccessful);
        Assert.Equal(entity, result.Data);
    }

    [Fact]
    public async void DeleteShouldReturnEntity()
    {
        var carRandomizer = new CarRandomizer();
        var entity = carRandomizer.PrepareRandomValue();

        var mockRepository = new Mock<IRepository<Car>>();
        mockRepository.Setup(r => r.DeleteAsync(entity))
            .ReturnsAsync(new OperationResult<Car>() { Data = entity });

        var service = new Service<Car>(mockRepository.Object);

        var result = await service.DeleteAsync(entity);
        
        Assert.True(result.IsSuccessful);
        Assert.Equal(entity, result.Data);
    }
}