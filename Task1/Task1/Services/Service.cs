using Exceptions.Exeptions;
using System.Security.Cryptography;
using System.Text;
using Task1.CacheSystem.Interfaces;
using Task1.FileSystem.Interfaces;
using Task1.Services.Interfaces;

namespace Task1.Services
{
    public class Service : IService
    {
        private readonly string _path = @"../Images";
        private readonly ICacheSystem _cache;
        private readonly IFileSystem _fileSystem;

        public Service(ICacheSystem cache, IFileSystem fileSystem)
        {
            _cache = cache;
            _fileSystem = fileSystem;
        }

        public async Task DeleteImage(string name)
        {
            var filePath = Path.Combine(_path, name);
            if (!string.IsNullOrEmpty(await _cache.GetValue(name)))
            {
                _fileSystem.DeleteImage(filePath);
                await _cache.RemoveValue(name);
            }
            else if (_fileSystem.ImageExist(filePath))
            {
                _fileSystem.DeleteImage(filePath);
            }
            return;
        }

        public async Task<byte[]> DownloadImage(string name)
        {
            var filePath = Path.Combine(_path, name);

            if (string.IsNullOrEmpty(await _cache.GetValue(name)))
            {
                if (!_fileSystem.ImageExist(filePath))
                {
                    throw new NotFoundException("File not found");
                }

                await _cache.SetValue(name, name);
            }

            return await _fileSystem.GetImage(filePath);
        }

        public async Task<List<string>> GetImagesNames()
        {
            var fileNames = _fileSystem.GetFiles(_path);

            foreach (var fileName in fileNames)
            {
                if (fileName != null && string.IsNullOrEmpty(await _cache.GetValue(fileName)))
                {
                    await _cache.SetValue(fileName, fileName);
                }
            }

            return fileNames!;
        }


        public async Task UploadImage(IFormFile image)
        {
            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            var fileByte = ms.ToArray();

            using SHA256 hasher = SHA256.Create();
            string name = GetHash(hasher, fileByte);

            var filePath = Path.Combine(_path, name);

            if (!string.IsNullOrEmpty(await _cache.GetValue(name)))
            {
                throw new ConflictException("File already exists");
            }
            else if (_fileSystem.ImageExist(filePath))
            {
                await _cache.SetValue(name, name);
                throw new ConflictException("File already exists");
            }

            await _fileSystem.AddImage(filePath, image);
            await _cache.SetValue(name, name);
            return;
        }

        private static string GetHash(SHA256 hasher, byte[] fileByte)
        {
            var hash = hasher.ComputeHash(fileByte);
            var sBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
