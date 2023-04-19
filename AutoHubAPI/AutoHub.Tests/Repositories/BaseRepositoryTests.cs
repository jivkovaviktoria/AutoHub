using AutoHub.Data;
using AutoHub.Data.Models;
using AutoHub.Tests.Randomizers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TryAtSoftware.Equalizer.Core;
using TryAtSoftware.Equalizer.Core.ProfileProviders;
using TryAtSoftware.Equalizer.Core.Profiles.General;
using TryAtSoftware.Randomizer.Core.Interfaces;
namespace AutoHub.Tests.Repositories;

public class BaseRepositoryTests
{
    private readonly IRandomizer<Car> _carRandomizer;
    private readonly Equalizer _equalizer;
    private TestDbContext _context;
    private Repository<Car> _repository;
    private readonly User _user;
    

    public BaseRepositoryTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("TestConnectionString");
        
        this._context = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(connectionString)
            .Options);
        
        var profileProvider = new DedicatedProfileProvider();
        profileProvider.AddProfile(new GeneralEqualizationProfile<Car>());
        this._equalizer = new Equalizer();
        this._equalizer.AddProfileProvider(profileProvider);

        this._carRandomizer = new CarRandomizer();
        this._repository = new Repository<Car>(this._context);
        
        this._user = new User() { Email = "test@gmail.com", Id = Guid.NewGuid().ToString(), UserName = "Test" };
    }

    [Fact]
    public async void TestTheDatabaseConnection()
    {
        await this._context.Database.EnsureDeletedAsync();
        await this._context.Database.EnsureCreatedAsync();

        this._repository = new Repository<Car>(this._context);
        
        Assert.NotNull(this._repository);
    }
    
    [Fact]
    public async void TestGetAsyncShouldReturnCar()
    {
        await this._context.Database.EnsureDeletedAsync();
        await this._context.Database.EnsureCreatedAsync();

        var car = this._carRandomizer.PrepareRandomValue();
        car.User = this._user;
        car.UserId = this._user.Id;

        await this._repository.CreateAsync(car);
        var result = await this._repository.GetAsync(car.Id);
        
        this._equalizer.AssertEquality(car, result.Data);
    }

    [Fact]
    public async void TestGetAsyncWithInvalidIdShouldNotReturnEntity()
    {
        await this._context.Database.EnsureDeletedAsync();
        await this._context.Database.EnsureCreatedAsync();

        var result = await this._repository.GetAsync(Guid.NewGuid());
        
        this._equalizer.AssertEquality(null, result.Data);
    }

    [Fact]
    public async void TestCreateAsyncShouldBeSuccessfull()
    {
        await this._context.Database.EnsureDeletedAsync();
        await this._context.Database.EnsureCreatedAsync();

        var car = this._carRandomizer.PrepareRandomValue();
        car.User = this._user;
        car.UserId = this._user.Id;

        var result = await this._repository.CreateAsync(car);
        
        this._equalizer.AssertEquality(true, result.IsSuccessful);
    }

    [Fact]
    public async void TestCreateAsyncShouldNotBeSuccessfull()
    {
        await this._context.Database.EnsureDeletedAsync();
        await this._context.Database.EnsureCreatedAsync();

        var car = this._carRandomizer.PrepareRandomValue();

        var result = await this._repository.CreateAsync(car);
        
        this._equalizer.AssertEquality(false, result.IsSuccessful);
    }

    [Fact]
    public async void GetManyAsyncShouldReturnCollectionOfCars()
    {
        await this._context.Database.EnsureDeletedAsync();
        await this._context.Database.EnsureCreatedAsync();

        for (int i = 0; i < 10; i++)
        {
            var car = this._carRandomizer.PrepareRandomValue();
            car.User = this._user;
            car.UserId = this._user.Id;
            
            await this._repository.CreateAsync(car);
        }

        var result = await this._repository.GetManyAsync();
        
        this._equalizer.AssertEquality(true, result.IsSuccessful);
        this._equalizer.AssertEquality(10, result.Data?.Count());
    }

    [Fact]
    public async void TestAnyAsyncWithValidCarShouldReturnTrue()
    {
        await this._context.Database.EnsureDeletedAsync();
        await this._context.Database.EnsureCreatedAsync();

        var car = this._carRandomizer.PrepareRandomValue();
        car.User = this._user;
        car.UserId = this._user.Id;

        await this._repository.CreateAsync(car);

        var result = await this._repository.AnyAsync(car.Id);
        
        this._equalizer.AssertEquality(true, result.IsSuccessful);
        this._equalizer.AssertEquality(true, result.Data);
    }

    [Fact]
    public async void TestAnyAsyncWithInvalidEntityShouldReturnFalse()
    {
        await this._context.Database.EnsureDeletedAsync();
        await this._context.Database.EnsureCreatedAsync();

        var result = await this._repository.AnyAsync(Guid.NewGuid());
        
        this._equalizer.AssertEquality(true, result.IsSuccessful);
        this._equalizer.AssertEquality(false, result.Data);
    }

    [Fact]
    public async void DeleteWithValidCarShouldBeSuccessfull()
    {
        await this._context.Database.EnsureDeletedAsync();
        await this._context.Database.EnsureCreatedAsync();

        var car = this._carRandomizer.PrepareRandomValue();
        car.User = this._user;
        car.UserId = this._user.Id;

        await this._repository.CreateAsync(car);

        var result = await this._repository.DeleteAsync(car);
        
        this._equalizer.AssertEquality(true, result.IsSuccessful);
        this._equalizer.AssertEquality(0, this._context.Cars.Count());
    }

    [Fact]
    public async void DeleteWithInvalidEntityShouldNotBeSuccessfull()
    {
        await this._context.Database.EnsureDeletedAsync();
        await this._context.Database.EnsureCreatedAsync();

        var car = this._carRandomizer.PrepareRandomValue();
        car.User = this._user;
        car.UserId = this._user.Id;

        var result = await this._repository.DeleteAsync(car);
        
        this._equalizer.AssertEquality(false, result.IsSuccessful);
    }
}