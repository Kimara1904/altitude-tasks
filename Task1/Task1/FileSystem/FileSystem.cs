using Task1.FileSystem.Interfaces;

namespace Task1.FileSystem
{
    public class FileSystem : IFileSystem
    {
        public async Task AddImage(string path, IFormFile image)
        {
            using var stream = File.Create(path);
            await image.CopyToAsync(stream);
        }

        public void DeleteImage(string path)
        {
            File.Delete(path);
        }


        public List<string?> GetFiles(string path)
        {
            return Directory.GetFiles(path)
                .Select(Path.GetFileName)
                .Where(name => name != null)
                .OrderBy(name => name)
                .ToList();
        }

        public async Task<byte[]> GetImage(string path)
        {
            var memory = new MemoryStream();
            using var stream = new FileStream(path, FileMode.Open);
            await stream.CopyToAsync(memory);
            memory.Position = 0;
            return memory.ToArray();
        }

        public bool ImageExist(string path)
        {
            return File.Exists(path);
        }
    }
}
