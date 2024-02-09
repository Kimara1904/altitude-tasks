namespace Task1.Services.Interfaces
{
    public interface IService
    {
        Task UploadImage(IFormFile image);
        Task<List<string>> GetImagesNames();
        Task DeleteImage(string name);
        Task<byte[]> DownloadImage(string name);
    }
}
