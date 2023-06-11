using AutoHub.API.Extensions;
using AutoHub.API.Models;
using AutoHub.Core.Contracts;
using AutoHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly UserManager<User> _userManager;

    public ReviewController(IReviewService reviewService, UserManager<User> userManager)
    {
        this._reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get([FromQuery] string userId)
    {
        var result = await this._reviewService.GetByUser(userId);
        if (!result.IsSuccessful) return this.Error(result);

        return this.Ok(result.Data);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create([FromBody]ReviewInputModel reviewInputModel)
    {
        var review = new Review() { Text = reviewInputModel.Text, UserId = reviewInputModel.UserId, Owner = await this.GetCurrentUser()};
        
        var result = await this._reviewService.CreateAsync(review);
        if (!result.IsSuccessful) return this.Error(result);

        return this.CreatedAtAction(nameof(Get), review.Id, review);
    }

    private async Task<User> GetCurrentUser()
    {
        var claims = HttpContext.User;
        var user = await this._userManager.FindByNameAsync(claims.Identity?.Name);
        return user;
    }
}