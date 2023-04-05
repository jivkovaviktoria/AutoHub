using AutoHub.Data.Contracts;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AutoHub.Data.Repositories;

public class CloudinaryRepository : ICloudinaryRepository
{
    private readonly IConfiguration _configuration;
    private readonly Account _account;

    public CloudinaryRepository(IConfiguration configuration)
    {
        this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        this._account = new Account(this._configuration.GetSection("Cloudinary")["Name"],
            this._configuration.GetSection("Cloudinary")["Key"],
            this._configuration.GetSection("Cloudinary")["Secret"]);
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