using AutoHub.API.Controllers;
using AutoHub.Core.Services;
using AutoHub.Data.Contracts;
using AutoHub.Data.Contracts.Repositories;
using AutoHub.Data.Models;
using AutoHub.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace AutoHub.Tests.Controllers;

public class ImagesControllerTests
{
    private readonly ImageService _imageService;
    private readonly Mock<IRepository<Image>> _imageRepository;
    private readonly Mock<ICloudinaryRepository> _cloudinaryRepository;
    private ImagesController _controller;

    public ImagesControllerTests()
    {
        var mockRepository = new Mock<IRepository<Image>>();
        this._imageRepository = mockRepository;
        
        this._cloudinaryRepository = new Mock<ICloudinaryRepository>();
        
        this._imageService = new ImageService(this._cloudinaryRepository.Object, this._imageRepository.Object);
    }

    [Fact]
    public async void TestUploadAsync()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.FileName).Returns("file1.jpg");

        this._cloudinaryRepository.Setup(cr => cr.UploadImage(file.Object))
            .ReturnsAsync("https://example.com/image.jpg");

        this._controller = new ImagesController(this._cloudinaryRepository.Object, this._imageService);
        var result = await this._controller.UploadAsync(file.Object) as JsonResult;

        var json = JsonConvert.SerializeObject(result.Value);
        var obj = JsonConvert.DeserializeObject<dynamic>(json);
        
        Assert.NotNull(obj);
        Assert.NotNull(obj.Link);

        var link = obj.Link;
        Assert.Equal(link.ToString(), "https://example.com/image.jpg");
    }

    [Fact]
    public async void TestUploadMany()
    {
        this._cloudinaryRepository.Setup(repo => repo.UploadImage(It.IsAny<IFormFile>()))
            .ReturnsAsync("https://example.com/image.jpg");
        
        this._controller = new ImagesController(this._cloudinaryRepository.Object, this._imageService);
        
        var file1 = new Mock<IFormFile>();
        file1.Setup(f => f.FileName).Returns("file1.jpg");
        
        var file2 = new Mock<IFormFile>();
        file2.Setup(f => f.FileName).Returns("file1.jpg");

        var result = await this._controller.UploadManyAsync(new List<IFormFile>() { file1.Object, file2.Object }) as OkObjectResult;
        Assert.NotNull(result);
        
        var urls = result.Value as List<string>;
        Assert.NotNull(urls);
        
        Assert.Equal("https://example.com/image.jpg", urls[0]);
        Assert.Equal("https://example.com/image.jpg", urls[1]);
    }
}