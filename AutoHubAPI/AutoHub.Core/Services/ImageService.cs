﻿using AutoHub.Data.Contracts;
using AutoHub.Data.Contracts.Repositories;
using AutoHub.Data.Models;
using Microsoft.AspNetCore.Http;

namespace AutoHub.Core.Services;

public class ImageService : Service<Image>
{
    private readonly ICloudinaryRepository _cloudinaryRepository;
    public ImageService(ICloudinaryRepository cloudinaryRepository, IRepository<Image> imagesRepository) : base(imagesRepository)
    {
        this._cloudinaryRepository = cloudinaryRepository ?? throw new ArgumentNullException(nameof(cloudinaryRepository));
    }

    public async Task<List<string>> AddRange(List<IFormFile> files)
    {
        List<string> links = new();
        foreach (var file in files)
        {
            var link = await this._cloudinaryRepository.UploadImage(file);
            links.Add(link);
        }

        return links;
    }
}