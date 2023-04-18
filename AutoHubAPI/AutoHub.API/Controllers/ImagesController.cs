using System.Net;
using AutoHub.API.Extensions;
using AutoHub.Core.Services;
using AutoHub.Data.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub.API.Controllers;

[ApiController]
public class ImagesController : ControllerBase
{
    private readonly ICloudinaryRepository _cloudinaryRepository;
    private readonly ImageService _imageService;

    public ImagesController(ICloudinaryRepository cloudinaryRepository, ImageService imageService)
    {
        this._cloudinaryRepository = cloudinaryRepository ?? throw new ArgumentNullException(nameof(cloudinaryRepository));
        this._imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
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

    [HttpPost]
    [Route("/UploadMany")]
    public async Task<IActionResult> UploadManyAsync(List<IFormFile> files)
    {
        var images = await this._imageService.AddRange(files);
        if (images is not null) return this.Ok(images);
        return this.BadRequest();
    }

    [HttpGet]
    [Route("/GetByCarId")]
    public async Task<IActionResult> GetByCarId(Guid carId)
    {
        var result = await this._imageService.GetManyAsync();
        if (!result.IsSuccessful) return this.Error(result);

        var images = result.Data?.Where(x => x.CarId == carId);
        return this.Ok(images);
    }
}