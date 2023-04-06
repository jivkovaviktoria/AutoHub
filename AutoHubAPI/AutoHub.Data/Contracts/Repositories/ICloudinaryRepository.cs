using Microsoft.AspNetCore.Http;

namespace AutoHub.Data.Contracts.Repositories;

public interface ICloudinaryRepository
{
    Task<string> UploadImage(IFormFile file);
}