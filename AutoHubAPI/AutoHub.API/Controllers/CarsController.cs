using AutoHub.Data.Contracts;
using AutoHub.Data.Models;
using Microsoft.AspNetCore.Mvc;
namespace AutoHub.API.Controllers;

[ApiController]
public class CarsController : ControllerBase
{
    private readonly IRepository<Car> _repository;

    public CarsController(IRepository<Car> repository)
    {
        this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    [Route("/Car")]
    public async Task<IActionResult> GetCar(Guid id)
    {
        var car = await this._repository.Get(id);
        
        if (car == null) return NotFound();
        return Ok(car);
    }

    [HttpGet]
    [Route("/AllCars")]
    public async Task<IActionResult> GetAllCars()
    {
        var cars = await this._repository.GetAll();
        return Ok(cars);
    }
}