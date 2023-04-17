using AutoHub.API.Extensions;
using AutoHub.Core.Contracts;
using AutoHub.Core.FilterDefinitions;
using AutoHub.Core.FilterDefinitions.Definitions;
using AutoHub.Data.Contracts;
using AutoHub.Data.Models;
using AutoHub.Data.Models.ViewModels;
using AutoHub.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.API.Controllers;

[ApiController]
public class CarsController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IService<Car> _carService;
    private readonly IFilteringService<Car> _filteringService;
    private readonly IRepository<Image> _imageRepository;
    private readonly IMapper _mapper;

    public CarsController(UserManager<User> userManager, IService<Car> carService, IRepository<Image> imageRepository, IMapper mapper, IFilteringService<Car> filteringService)
    {
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this._imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
        this._carService = carService ?? throw new ArgumentNullException(nameof(carService));
        this._filteringService = filteringService ?? throw new ArgumentNullException(nameof(filteringService));
    }

    [HttpGet, Authorize]
    [Route("/Car")]
    public async Task<IActionResult> GetCar(Guid id)
    {
        var result = await this._carService.GetAsync(id);
        
        if (!result.IsSuccessful) return this.Error(result);
        if (result.Data is null) return this.NotFound();
        
        var car = result.Data;
        return this.Ok(car);
    }

    [HttpGet, Authorize]
    [Route("/AllCars")]
    public async Task<IActionResult> GetAllCars([FromQuery]GlobalCarFilter? filter)
    {
        var cars = await this._carService.GetManyAsync();
        if (!cars.IsSuccessful) return this.Error(cars);

        if (filter != null)
        {
            var filteredCars = await this._filteringService.Filter(cars.Data.OrEmptyIfNull(), filter);
            var result = filteredCars.Data.OrEmptyIfNull().Select(x => this.ToViewModel(x));
            return this.Ok(result);
        }
        
        return this.Ok(cars.Data);
    }
    
    [HttpGet]
    [Route("/OrderCars")]
    public async Task<IActionResult> OrderCars([FromQuery]OrderDefinition order)
    {
        var result = await this._filteringService.OrderBy(order);
        if (!result.IsSuccessful) return this.Error(result);
        
        return this.Ok(result.Data);
    }

    [HttpGet, Authorize]
    [Route("GetCarsByUser")]
    public async Task<IActionResult> GetCarsByUser()
    {
        var claims = HttpContext.User;
        
        var userName = claims.Identity?.Name;
        if (string.IsNullOrEmpty(userName)) return BadRequest("Invalid user name.");
        
        var user = await this._userManager.Users
            .Where(u => u.UserName == userName)
            .Include(x => x.Cars)
            .Select(u => new { Cars = u.Cars.Select(c => new { c.Model, c.Brand, c.Year, c.Price, c.ImageUrl, c.Description }) })
            .ToListAsync();

        return this.Ok(user);
    }

    [HttpPost, Authorize]
    [Route("/Add")]
    public async Task<IActionResult> Add(CarInfoViewModel entity)
    {
        var dbModel = this.ToDatabaseModel(entity);
        var car = await this.SetUserAsync(dbModel);
        
        List<Image> images = new();
        foreach (var link in entity.Images)
        {
            var image = new Image() { Id = Guid.NewGuid(), Url = link, CarId = car.Id };
            await this._imageRepository.CreateAsync(image);
            images.Add(image);
        }
        
        car.Images = images;
        
        var result = await this._carService.CreateAsync(car);
        if (!result.IsSuccessful) return this.Error(result);
        
        return CreatedAtAction(nameof(GetCar), new {id = car.Id}, car);
    }

    [HttpDelete, Authorize]
    [Route("/Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await this._carService.GetAsync(id);
        if (!result.IsSuccessful) return this.Error(result);

        var car = result.Data;
        if (car is null) return this.NotFound();

        await this._carService.DeleteAsync(car);
        return this.Ok();
    }    
    
    private object ToViewModel(Car car) => this._mapper.Map<CarInfoViewModel>(car);

    private Car ToDatabaseModel(CarInfoViewModel viewModel)
    {
        var car = this._mapper.Map<Car>(viewModel);
        car.Id = Guid.NewGuid();
        return car;
    }

    private async Task<Car> SetUserAsync(Car car)
    {
        var claims = HttpContext.User;
        var user = await this._userManager.FindByNameAsync(claims.Identity?.Name);
        
        user.Cars.Add(car);
        car.UserId = user.Id;
        return car;
    }
}