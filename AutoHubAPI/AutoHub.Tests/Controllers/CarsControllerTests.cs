using AutoHub.API.Controllers;
using AutoHub.Core.Contracts;
using AutoHub.Core.FilterDefinitions;
using AutoHub.Core.FilterDefinitions.Definitions;
using AutoHub.Data.Contracts;
using AutoHub.Data.Models;
using AutoHub.Tests.Randomizers;
using AutoHub.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TryAtSoftware.Equalizer.Core;
using TryAtSoftware.Equalizer.Core.ProfileProviders;
using TryAtSoftware.Equalizer.Core.Profiles.General;

namespace AutoHub.Tests.Controllers;

public class CarsControllerTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IService<Car>> _mockCarService;
    private readonly Mock<IFilteringService<Car>> _mockFilteringService;
    private readonly Mock<IRepository<Image>> _mockImageRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CarsController _carController;
    
    private readonly CarRandomizer _carRandomizer;
    private readonly Equalizer _equalizer;

    public CarsControllerTests()
    {
        Mock<UserManager<User>> mockUserManager;
        this._mockUserManager = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
        
        this._mockCarService = new Mock<IService<Car>>();
        this._mockFilteringService = new Mock<IFilteringService<Car>>();
        this._mockImageRepository = new Mock<IRepository<Image>>();
        this._mockMapper = new Mock<IMapper>();
        this._carController = new CarsController(this._mockUserManager.Object,
            this._mockCarService.Object,
            this._mockImageRepository.Object,
            this._mockMapper.Object, this._mockFilteringService.Object);

        this._carRandomizer = new CarRandomizer();
        
        var profileProvider = new DedicatedProfileProvider();
        profileProvider.AddProfile(new GeneralEqualizationProfile<Car>());
        this._equalizer = new Equalizer();
        this._equalizer.AddProfileProvider(profileProvider);
    }

    [Fact]
    public async void GetCarWithValidIdShouldReturnCar()
    {
        var expectedEntity = new Car() { Id = Guid.NewGuid(), Model = "Rs7", Brand = "Audi", Year = 2020 };
        
        this._mockCarService.Setup(s => s.GetAsync(expectedEntity.Id))
            .ReturnsAsync(new OperationResult<Car> { Data = expectedEntity });
        
        var result = await this._carController.GetCar(expectedEntity.Id);
        
        Assert.IsType<OkObjectResult>(result);
        
        var okResult = result as OkObjectResult;
        Assert.IsType<Car>(okResult?.Value);
        
        var car = okResult?.Value as Car;
        
        this._equalizer.AssertEquality(expectedEntity, car);
    }

    [Fact]
    public async void GetCarWithInvalidIdShouldReturnProblem()
    {
        var id = Guid.NewGuid();
        
        this._mockCarService.Setup(s => s.GetAsync(id))
            .ReturnsAsync(new OperationResult<Car> { Data = null });
        
        var result = await this._carController.GetCar(id);
        
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void GetAllCarsShouldReturnCollectionOfCars()
    {
        var expected = new List<Car>();
        for(int i = 0; i <5; i++) expected.Add(this._carRandomizer.PrepareRandomValue());

        this._mockCarService.Setup(s => s.GetManyAsync())
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>() { Data = expected });

        var result = await this._carController.GetAllCars(null);
        
        var okResult = result as OkObjectResult;
        Assert.IsType<List<Car>>(okResult?.Value);
        
        var cars = okResult?.Value as List<Car>;
        
        this._equalizer.AssertEquality(expected, cars);
    }

    [Fact]
    public async void OrderCarsShouldOrderCarsCorrectly()
    {
        var expected = new List<Car>();
        for(int i = 0; i <5; i++) expected.Add(this._carRandomizer.PrepareRandomValue());

        var orderDefinition = new OrderDefinition() { Property = "Price", IsAscending = true };

        this._mockFilteringService.Setup(s => s.OrderBy(orderDefinition))
            .ReturnsAsync(new OperationResult<IEnumerable<Car>>(){Data = expected.OrderBy(x => x.Price)});

        var result = await this._carController.OrderCars(orderDefinition);
        
        var okResult = result as OkObjectResult;
        Assert.IsAssignableFrom<IOrderedEnumerable<Car>>(okResult?.Value);
        
        var cars = okResult?.Value as IOrderedEnumerable<Car>;
        
        this._equalizer.AssertEquality(expected.OrderBy(x => x.Price), cars);
    }

    [Fact]
    public async void DeleteShouldWorkCorrectly()
    {
        var entity = this._carRandomizer.PrepareRandomValue();

        this._mockCarService.Setup(s => s.DeleteAsync(entity))
            .ReturnsAsync(new OperationResult<Car>() { Data = entity });

        this._mockCarService.Setup(s => s.GetAsync(entity.Id))
            .ReturnsAsync(new OperationResult<Car>(){Data = entity});

        var result = await this._carController.Delete(entity.Id);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async void DeleteWithInvalidIdShouldReturnNotFound()
    {
        var entity = this._carRandomizer.PrepareRandomValue();

        this._mockCarService.Setup(s => s.DeleteAsync(entity))
            .ReturnsAsync(new OperationResult<Car>() { Data = null });

        this._mockCarService.Setup(s => s.GetAsync(entity.Id))
            .ReturnsAsync(new OperationResult<Car>(){Data = null});

        var result = await this._carController.Delete(entity.Id);

        Assert.IsType<NotFoundResult>(result);
    }
}