using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System;

namespace Restaurant.Application.Common.ImageServices
{
    public class ImageUploaderService : IImageUploaderService
    {
        private readonly Cloudinary _cloudinary;

        public ImageUploaderService(IConfiguration configuration)
        {
            var cloud = configuration["Cloudinary:Cloud"]
                ?? throw new InvalidOperationException("Cloudinary:Cloud is not configured");
            var apiKey = configuration["Cloudinary:ApiKey"]
                ?? throw new InvalidOperationException("Cloudinary:ApiKey is not configured");
            var apiSecret = configuration["Cloudinary:ApiSecret"]
                ?? throw new InvalidOperationException("Cloudinary:ApiSecret is not configured");

            var account = new Account
            {
                Cloud = cloud,
                ApiKey = apiKey,
                ApiSecret = apiSecret
            };

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<string> UploadImageAsync(byte[] imageData, string fileName, string tenantId, string folderName, string contentType)
        {
            if (imageData == null || imageData.Length == 0)
                throw new ArgumentException("Image data cannot be empty", nameof(imageData));

            if (!contentType.StartsWith("image/"))
                throw new ArgumentException($"Invalid content type: {contentType}. Only image types are allowed.", nameof(contentType));

            string uniqueFileName = $"{Guid.NewGuid()}-{fileName}";

            using var stream = new MemoryStream(imageData);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(uniqueFileName, stream),
                Folder = $"restaurant-app/tenant-{tenantId}/{folderName}"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl.ToString();
        }
    }
}

