using System.Reflection;
using AutoHub.API.Extensions;
using AutoHub.Core.Contracts;
using AutoHub.Data.Models;
using AutoHub.Data.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AutoHub.API.Controllers;

[ApiController]
public class CarsController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IService<Car> _carService;
    private readonly IMapper _mapper;

    public CarsController(UserManager<User> userManager, IService<Car> carService, IMapper mapper)
    {
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this._carService = carService ?? throw new ArgumentNullException(nameof(carService));
    }

    [HttpGet, Authorize]
    [Route("/Car")]
    public async Task<IActionResult> GetCar(Guid id)
    {
        var result = await this._carService.GetAsync(id);
        
        if (!result.IsSuccessful) return this.Error(result);
            
        var car = result.Data;
        return this.Ok(car);
    }

    [HttpGet, Authorize]
    [Route("/AllCars")]
    public async Task<IActionResult> GetAllCars()
    {
        var cars = await this._carService.GetManyAsync();
        
        if (!cars.IsSuccessful) return this.Error(cars);

        var result = cars.Data.Select(x => this.ToViewModel(x));
        return this.Ok(result);
    }
    
    [HttpGet]
    [Route("/OrderCars")]
    public async Task<IActionResult> OrderCars([FromQuery]OrderDefinition order)
    {
        var result = await this._carService.GetManyAsync();

        if (!result.IsSuccessful) return this.Error(result);
        
        var cars = result.Data;

        var prop = typeof(Car).GetProperty(order.Property,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (order.IsAscending) cars = cars.OrderBy(x => prop.GetValue(x, null)).ToList();
        else cars = cars.OrderByDescending(x => prop.GetValue(x, null)).ToList();

        return this.Ok(cars);
    }

    [HttpGet, Authorize]
    [Route("GetCarsByUser")]
    public async Task<IActionResult> GetCarsByUser()
    {
        var claims = HttpContext.User;
        var user = await this._userManager.Users
            .Where(u => u.UserName == claims.Identity.Name)
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

        var result = await this._carService.CreateAsync(car);
        if (!result.IsSuccessful) return this.Error(result);
        
        return CreatedAtAction(nameof(GetCar), new {id = car.Id}, car);
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