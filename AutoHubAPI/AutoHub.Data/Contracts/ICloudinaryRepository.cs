using Microsoft.AspNetCore.Http;

namespace AutoHub.Data.Contracts;

public interface ICloudinaryRepository
{
    Task<string> UploadImage(IFormFile file);
}