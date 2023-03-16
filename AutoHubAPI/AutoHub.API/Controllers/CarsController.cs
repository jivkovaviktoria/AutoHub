using AutoHub.Data.Contracts;
using AutoHub.Data.Models;
using AutoHub.Data.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AutoHub.API.Controllers;

[ApiController]
public class CarsController : ControllerBase
{
    private readonly IRepository<Car> _repository;
    private readonly IMapper _mapper;

    public CarsController(IRepository<Car> repository, IMapper mapper)
    {
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

    [HttpPost, Authorize]
    [Route("/Add")]
    public async Task<IActionResult> Add(CarInfoViewModel entity)
    {
        var car = this._mapper.Map<Car>(entity);
        car.Id = new Guid();
        
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