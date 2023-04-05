using AutoHub.Data.Contracts;
using AutoHub.Data.Models;
using Microsoft.AspNetCore.Http;

namespace AutoHub.Core.Services;

public class ImageService : Service<Image>
{
    private readonly ICloudinaryRepository _cloudinaryRepository;
    private readonly IRepository<Car> _carRepository;
    private readonly IRepository<Image> _imageRepository;

    public ImageService(ICloudinaryRepository cloudinaryRepository, IRepository<Image> imagesRepository, IRepository<Car> carRepository) : base(imagesRepository)
    {
        this._cloudinaryRepository = cloudinaryRepository ?? throw new ArgumentNullException(nameof(cloudinaryRepository));
        this._carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        this._imageRepository = imagesRepository ?? throw new ArgumentNullException(nameof(imagesRepository));
    }

    public async Task<List<string>> AddRange(List<IFormFile> files, Guid carId)
    {
        var result = await this._carRepository.GetAsync(carId);
        var car = result.Data;
        
        List<string> links = new();
        foreach (var file in files)
        {
            var link = await this._cloudinaryRepository.UploadImage(file);
            links.Add(link);

            var image = new Image() { Id = Guid.NewGuid(), Url = link, CarId = car.Id };
            await this._imageRepository.CreateAsync(image);
        }

        return links;
    }
}