using Microsoft.Extensions.Caching.Distributed;
using Task1.CacheSystem.Interfaces;

namespace Task1.CacheSystem
{
    public class CacheSystem : ICacheSystem
    {
        private readonly IDistributedCache _cache;

        public CacheSystem(IDistributedCache cacheSystem)
        {
            _cache = cacheSystem;
        }

        public async Task<string?> GetValue(string key)
        {
            return await _cache.GetStringAsync(key);
        }

        public async Task RemoveValue(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task SetValue(string key, string value)
        {
            await _cache.SetStringAsync(key, value);
        }
    }
}
