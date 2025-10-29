namespace PosterBoi.Core.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName);
    }
}
