using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PosterBoi.Core.Configs;
using Microsoft.Extensions.Options;
using PosterBoi.Core.Interfaces.Services;

namespace PosterBoi.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> settings)
        {
            var acc = new Account(settings.Value.CloudName, settings.Value.ApiKey, settings.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "PosterBoi/Posts",
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = true,
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                return uploadResult.SecureUrl.ToString();

            throw new Exception("Cloudinary upload failed");
        }
    }
}
