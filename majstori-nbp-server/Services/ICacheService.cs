using StackExchange.Redis;

namespace majstori_nbp_server.Services;

public interface ICacheService
{
    Task<string?> GetDataAsync(string key);
    Task<HashEntry[]> GetHashDataAsync(string key);
    Task<bool> SetDataAsync(string key, string value);
    Task<bool> SetHashDataAsync<T>(string key, T value);
    Task<bool> SetKeyExpiryTime(string key, TimeSpan expiryTime);
    Task<bool> RemoveData(string key);
}