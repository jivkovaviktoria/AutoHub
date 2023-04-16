using AutoHub.API.Extensions;
using AutoHub.Core.Contracts;
using AutoHub.Data;
using AutoHub.Data.Models;
using AutoHub.Data.Models.ViewModels;
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
        
        var userName = claims.Identity?.Name;
        if (string.IsNullOrEmpty(userName)) return BadRequest("Invalid user name.");
        
        var user = await this._userManager.Users
            .Where(u => u.UserName == userName)
            .Include(x => x.FavouriteCars)
            .Select(us => new
            {
                Cars = us.FavouriteCars.Select(x => ToViewModel(x, this._mapper))
            })
            .ToListAsync();

        return Ok(user);
    }

    [HttpPost, Authorize]
    [Route("/AddToFavourite")]
    public async Task<IActionResult> AddToFavourite(Guid carId)
    {
        var result = await this._carService.GetAsync(carId);
        if (!result.IsSuccessful) return this.Error(result);

        var car = result.Data;
        if (car is null) return this.NotFound(car);
        
        var user = await this.GetUser();
        if (user is null) return this.BadRequest("User not found.");
        
        user.FavouriteCars.Add(car);
        car.UsersFavourite.Add(user);
        
        await this._context.SaveChangesAsync();
        return this.Ok();
    }

    [HttpGet]
    [Route("/User")]
    public new async Task<IActionResult> User()
    {
        var user = await this.GetUser();
        
        if (user is null) return this.NotFound();
        return this.Ok(user);
    }

    private async Task<User?> GetUser()
    {
        var claims = HttpContext.User;
        var user = await this._userManager.FindByNameAsync(claims.Identity?.Name);
        
        return user;
    }

    private static object ToViewModel(Car car, IMapper mapper)
    {
        var result = mapper.Map<CarInfoViewModel>(car);
        return result;
    }
}