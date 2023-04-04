using System.Net;
using AutoHub.Data.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub.API.Controllers;

[ApiController]
public class ImagesController : ControllerBase
{
    private readonly ICloudinaryRepository _cloudinaryRepository;

    public ImagesController(ICloudinaryRepository cloudinaryRepository)
    {
        this._cloudinaryRepository = cloudinaryRepository ?? throw new ArgumentNullException(nameof(cloudinaryRepository));
    }

    [HttpPost]
    [Route("/Upload")]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        var imageUrl = await this._cloudinaryRepository.UploadImage(file);

        if (imageUrl is null)
            return this.Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);

        return new JsonResult(new { Link = imageUrl });
    }
}