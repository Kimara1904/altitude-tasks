namespace Task1.CacheSystem.Interfaces
{
    public interface ICacheSystem
    {
        Task<string?> GetValue(string key);
        Task SetValue(string key, string value);
        Task RemoveValue(string key);
    }
}
