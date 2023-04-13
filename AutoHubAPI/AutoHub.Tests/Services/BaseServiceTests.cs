using AutoHub.Core.Services;
using AutoHub.Data.Contracts;
using AutoHub.Data.Models;
using AutoHub.Tests.Randomizers;
using AutoHub.Utilities;
using Moq;
using TryAtSoftware.Equalizer.Core;
using TryAtSoftware.Equalizer.Core.ProfileProviders;
using TryAtSoftware.Equalizer.Core.Profiles.General;
using TryAtSoftware.Randomizer.Core.Primitives;

namespace AutoHub.Tests.Services;

public class BaseServiceTests
{
    private readonly Equalizer _equalizer;
    private readonly Mock<IRepository<Car>> _mockRepository;
    private readonly CarRandomizer _carRandomizer;

    public BaseServiceTests()
    {
        var profileProvider = new DedicatedProfileProvider();
        profileProvider.AddProfile(new GeneralEqualizationProfile<Car>());
        
        this._equalizer = new Equalizer();
        this._equalizer.AddProfileProvider(profileProvider);

        this._carRandomizer = new CarRandomizer();
        this._mockRepository = new Mock<IRepository<Car>>();
    }
    
    [Fact]
    public async void GetByWithValidIdReturnsEntity()
    {
        var expectedEntity = this._carRandomizer.PrepareRandomValue();

        this._mockRepository.Setup(r => r.GetAsync(expectedEntity.Id))
            .ReturnsAsync(new OperationResult<Car>(){Data = expectedEntity});

        var service = new Service<Car>(this._mockRepository.Object);

        var result = await service.GetAsync(expectedEntity.Id);
        
        Assert.True(result.IsSuccessful);
        this._equalizer.AssertEquality(expectedEntity, result.Data);
    }

    [Fact]
    public async void GetByIdWithInvalidIdShouldNotReturnEntity()
    {
        var guidRandomizer = new GuidRandomizer();
        var entityId = guidRandomizer.PrepareRandomValue();
        
        this._mockRepository.Setup(r => r.GetAsync(entityId)).ReturnsAsync(new OperationResult<Car>(){Data = null});

        var service = new Service<Car>(this._mockRepository.Object);

        var result = await service.GetAsync(entityId);
        
        Assert.True(result.IsSuccessful);
        this._equalizer.AssertEquality(null, result.Data);
    }

    [Fact]
    public async void GetManyShouldReturnCollectionOfEntities()
    {
        List<Car> cars = new List<Car>();
        for(int i = 0; i < 5; i++) cars.Add(this._carRandomizer.PrepareRandomValue());
        
        this._mockRepository.Setup(r => r.GetManyAsync()).ReturnsAsync(new OperationResult<IEnumerable<Car>>(){Data = cars});

        var service = new Service<Car>(this._mockRepository.Object);

        var result = await service.GetManyAsync();
        
        Assert.True(result.IsSuccessful);
        this._equalizer.AssertEquality(cars, result.Data);
    }

    [Fact]
    public async void GetManyShouldReturnEmptyCollection()
    {
        this._mockRepository.Setup(r => r.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>(){Data = Enumerable.Empty<Car>()});

        var service = new Service<Car>(this._mockRepository.Object);

        var result = await service.GetManyAsync();
        
        Assert.True(result.IsSuccessful);
        this._equalizer.AssertEquality(Enumerable.Empty<Car>(), result.Data);
    }

    [Fact]
    public async void AnyAsyncShouldReturnTrue()
    {
        var entity = this._carRandomizer.PrepareRandomValue();

        this._mockRepository.Setup(r => r.AnyAsync(entity.Id)).ReturnsAsync(new OperationResult<bool>(){Data = true});

        var service = new Service<Car>(this._mockRepository.Object);

        var result = await service.AnyAsync(entity.Id);
        
        Assert.True(result.IsSuccessful);
        Assert.True(result.Data);
    }

    [Fact]
    public async void AnyAsyncShouldReturnFalse()
    {
        var guidRandomizer = new GuidRandomizer();
        var entityId = guidRandomizer.PrepareRandomValue();
        
        this._mockRepository.Setup(r => r.AnyAsync(entityId))
            .ReturnsAsync(new OperationResult<bool>() { Data = false });

        var service = new Service<Car>(this._mockRepository.Object);

        var result = await service.AnyAsync(entityId);
        
        Assert.True(result.IsSuccessful);
        Assert.False(result.Data);
    }

    [Fact]
    public async void CreateShouldReturnEntity()
    {
        var expectedEntity = this._carRandomizer.PrepareRandomValue();

        this._mockRepository.Setup(r => r.CreateAsync(expectedEntity))
            .ReturnsAsync(new OperationResult<Car>() { Data = expectedEntity });

        var service = new Service<Car>(this._mockRepository.Object);

        var result = await service.CreateAsync(expectedEntity);
        
        Assert.True(result.IsSuccessful);
        this._equalizer.AssertEquality(expectedEntity, result.Data);
    }

    [Fact]
    public async void DeleteShouldReturnEntity()
    {
        var entity = this._carRandomizer.PrepareRandomValue();

        this._mockRepository.Setup(r => r.DeleteAsync(entity))
            .ReturnsAsync(new OperationResult<Car>() { Data = entity });

        var service = new Service<Car>(this._mockRepository.Object);

        var result = await service.DeleteAsync(entity);
        
        Assert.True(result.IsSuccessful);
        this._equalizer.AssertEquality(entity, result.Data);
    }
}