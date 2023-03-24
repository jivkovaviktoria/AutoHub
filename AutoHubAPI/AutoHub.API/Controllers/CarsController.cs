using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoHub.Data.Contracts;
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
    private readonly IRepository<Car> _repository;
    private readonly IMapper _mapper;

    public CarsController(UserManager<User> userManager, IRepository<Car> repository, IMapper mapper)
    {
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet, Authorize]
    [Route("/Car")]
    public async Task<IActionResult> GetCar(Guid id)
    {
        var car = await this._repository.Get(id);
        
        if (car == null) return NotFound();
        return Ok(car);
    }

    [HttpGet, Authorize]
    [Route("/AllCars")]
    public async Task<IActionResult> GetAllCars()
    {
        var cars = await this._repository.GetAll();
        return Ok(cars);
    }

    [HttpGet, Authorize]
    [Route("/GetAllCarsByUser")]
    public async Task<IActionResult> GetCarsByUser()
    {
        var cp = HttpContext.User;
        var userEmail = cp.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        var user = await this._userManager.Users.Include(x => x.Cars)
            .FirstOrDefaultAsync(x => x.Email == userEmail);
        
        return Ok(user.Cars);
    }

    [HttpPost, Authorize]
    [Route("/Add")]
    public async Task<IActionResult> Add(CarInfoViewModel entity)
    {
        var car = this._mapper.Map<Car>(entity);
        car.Id = new Guid();

        var claims = HttpContext.User;
        var user = await this._userManager.FindByEmailAsync("test2@abv.bg");
        
        user.Cars.Add(car);
        car.UserId = user.Id;
        
        await this._repository.Add(car);
        
        return CreatedAtAction(nameof(GetCar), new {id = car.Id}, car);
    }

    [HttpGet, Authorize]
    [Route("/GetCarsOrdered")]
    public async Task<IActionResult> GetCarsOrdered(string property, string direction)
    {
        var result = await this._repository.OrderCars(property, direction);
        return Ok(result);
    }
}