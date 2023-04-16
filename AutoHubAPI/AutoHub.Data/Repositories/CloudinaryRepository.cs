using AutoHub.Data.Contracts.Repositories;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AutoHub.Data.Repositories;

public class CloudinaryRepository : ICloudinaryRepository
{
    private readonly Account _account;

    public CloudinaryRepository(IConfiguration configuration)
    {
        IConfigurationSection config = configuration.GetSection("Cloudinary");

        this._account = new Account(config.GetSection("Cloudinary")["Name"],
            config.GetSection("Cloudinary")["Key"],
            config.GetSection("Cloudinary")["Secret"]);
    }
    
    public async Task<string> UploadImage(IFormFile file)
    {
        var client = new Cloudinary(this._account);

        var uploadParams = new ImageUploadParams()
            { File = new FileDescription(file.FileName, file.OpenReadStream()), DisplayName = file.FileName };

        var uploadResult = await client.UploadAsync(uploadParams);

        return uploadResult.SecureUrl.ToString();
    }
}