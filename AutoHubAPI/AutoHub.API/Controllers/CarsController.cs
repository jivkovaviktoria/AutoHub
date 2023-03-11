using AutoHub.Data.Contracts;
using AutoHub.Data.Models;
using AutoMapper;
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