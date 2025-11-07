namespace PosterBoi.Core.Interfaces.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName);
    }
}
