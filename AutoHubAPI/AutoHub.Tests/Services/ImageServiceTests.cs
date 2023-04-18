using AutoHub.Core.Services;
using AutoHub.Data.Contracts;
using AutoHub.Data.Contracts.Repositories;
using AutoHub.Data.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AutoHub.Tests.Services;

public class ImageServiceTests
{
    [Fact]
    public async void TestAddRange()
    {
        var file1 = new Mock<IFormFile>();
        file1.Setup(f => f.FileName).Returns("file1.jpg");
        file1.Setup(f => f.Length).Returns(1024);

        var file2 = new Mock<IFormFile>();
        file2.Setup(f => f.FileName).Returns("file2.jpg");
        file2.Setup(f => f.Length).Returns(2048);
        
        var mockRepository = new Mock<ICloudinaryRepository>();
        var mockImageRepository = new Mock<IRepository<Image>>();

        mockRepository.Setup(repo => repo.UploadImage(It.IsAny<IFormFile>()))
            .ReturnsAsync("https://example.com/image.jpg");

        var service = new ImageService(mockRepository.Object, mockImageRepository.Object);
        
        var files = new List<IFormFile> { file1.Object, file2.Object };
        var result = await service.AddRange(files);

        Assert.Equal(2, result.Count);
        Assert.Contains("https://example.com/image.jpg", result);
    }
}