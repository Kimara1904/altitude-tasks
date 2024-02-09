namespace Task1.FileSystem.Interfaces
{
    public interface IFileSystem
    {
        Task AddImage(string path, IFormFile image);
        void DeleteImage(string path);
        Task<byte[]> GetImage(string path);
        bool ImageExist(string path);
        List<string?> GetFiles(string path);
    }
}
