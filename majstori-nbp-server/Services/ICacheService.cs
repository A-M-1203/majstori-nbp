using StackExchange.Redis;

namespace majstori_nbp_server.Services;

public interface ICacheService
{
    IAsyncEnumerable<string> GetAllKeysAsync(string keyPattern);
    IAsyncEnumerable<string> GetAllStringsAsync(string keyPattern);
    Task<string?> GetStringAsync(string key);
    Task<bool> CreateOrUpdateStringAsync(string key, string value);
    Task<bool> CreateOrUpdateStringWithExpiryAsync(string key, string value, TimeSpan expiry);
    Task<bool> DeleteStringAsync(string key);

    IAsyncEnumerable<string> GetAllSetDataAsync(string key);
    Task<bool> SetDataExistsAsync(string key, string value);
    Task<bool> CreateSetDataAsync(string key, string value);
    Task<bool> UpdateSetDataAsync(string key, string oldValue, string newValue);
    Task<bool> DeleteSetDataAsync(string key, string value);

    IAsyncEnumerable<double> GetAllSortedSetData(string key, long pageSize, Order order);
    Task<double> CreateOrIncrementSortedSetDataAsync(string key, string value, int score);
    Task<bool> UpdateSortedSetDataAsync(string key, string oldValue, string newValue);
    Task<bool> DeleteSortedSetDataAsync(string key, string value);

    IAsyncEnumerable<List<HashEntry>> GetAllHashDataAsync(string keyPattern);
    Task<List<HashEntry>?> GetHashDataAsync(string key);
    Task<List<HashEntry>?> CreateOrUpdateHashDataAsync<T>(string key, T value);
    Task<bool> DeleteHashDataAsync(string key);
    Task<bool> IsHashKeyAsync(string key);

    Task<bool> SetKeyExpiryTimeAsync(string key, TimeSpan expiryTime);
}