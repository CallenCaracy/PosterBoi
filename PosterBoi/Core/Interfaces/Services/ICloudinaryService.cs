using PosterBoi.Core.Configs;

namespace PosterBoi.Core.Interfaces.Services
{
    public interface ICloudinaryService
    {
        Task<Result<string>> UploadAsync(Stream fileStream, string fileName);
    }
}
