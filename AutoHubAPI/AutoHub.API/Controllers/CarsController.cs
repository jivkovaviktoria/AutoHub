using AutoHub.API.Extensions;
using AutoHub.Core.Contracts;
using AutoHub.Data.Models;
using AutoHub.Data.ViewModels;
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
        var car = await this._carService.GetAsync(id);

        var result = car.Data;
        return this.Ok(result);
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

    [HttpGet, Authorize]
    [Route("GetCarsByUser")]
    public async Task<IActionResult> GetCarsByUser()
    {
        var claims = HttpContext.User;
        var user = await this._userManager.Users
            .Where(u => u.UserName == claims.Identity.Name)
            .Include(x => x.Cars)
            .Select(u => new { Cars = u.Cars.Select(c => new { c.Model, c.Brand, c.Year, c.Price }) })
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