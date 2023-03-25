using AutoHub.Core.Contracts;
using AutoHub.Data;
using AutoHub.Data.Models;
using AutoHub.Data.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.API.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IService<Car> _carService;
    private readonly AutoHubDbContext _context;

    public UserController(UserManager<User> userManager, IMapper mapper, IService<Car> carService, AutoHubDbContext context)
    {
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this._carService = carService ?? throw new ArgumentNullException(nameof(carService));
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    [HttpGet, Authorize]
    [Route("/GetFavourite")]
    public async Task<IActionResult> GetFavourite()
    {
        var claims = HttpContext.User;
        var user = await this._userManager.Users
            .Where(u => u.UserName == claims.Identity.Name)
            .Include(x => x.FavouriteCars)
            .Select(u => new
            {
                Cars = u.FavouriteCars.Select(x => new {x.Model, x.Brand, x.Year, x.Price, x.Id})
            })
            .ToListAsync();

        return Ok(user);
    }

    [HttpPost, Authorize]
    [Route("/AddToFavourite")]
    public async Task<IActionResult> AddToFavourite(Guid carId)
    {
        var car = await this._carService.GetAsync(carId);
        if (car is null || !car.IsSuccessful) return this.NotFound();
    
        var user = await this.GetUser();
        user.FavouriteCars.Add(car.Data);
        car.Data.UsersFavourite.Add(user);
        return this.Ok();
    }

    private async Task<User> GetUser()
    {
        var claims = HttpContext.User;
        var user = await this._userManager.FindByNameAsync(claims.Identity?.Name);
        
        return user;
    }
    
    private Car ToDatabaseModel(CarInfoViewModel viewModel)
    {
        var car = this._mapper.Map<Car>(viewModel);
        car.Id = Guid.NewGuid();
        return car;
    }
    
    private object ToViewModel(Car car) => this._mapper.Map<CarInfoViewModel>(car);
}