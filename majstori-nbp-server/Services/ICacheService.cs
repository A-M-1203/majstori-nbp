using StackExchange.Redis;

namespace majstori_nbp_server.Services;

public interface ICacheService
{
    IAsyncEnumerable<string> GetAllDataAsync(string keyPattern);
    Task<string?> GetDataAsync(string key);
    Task<bool> CreateDataAsync(string key, string value);
    Task<bool> CreateDataWithExpiryAsync(string key, string value, TimeSpan expiry);
    Task<bool> UpdateDataAsync(string key, string newValue);
    Task<bool> UpdateDataWithExpiryAsync(string key, string newValue, TimeSpan expiry);
    Task<bool> DeleteDataAsync(string key);
    
    //dodato
    Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null);
    Task<string?> GetStringAsync(string key);
    
    IEnumerable<string> GetAllSetData(string key);
    Task<bool> SetDataExistsAsync(string key, string value);
    Task<bool> CreateSetDataAsync(string key, string value);
    Task<bool> UpdateSetDataAsync(string key, string oldValue, string newValue);
    Task<bool> DeleteSetDataAsync(string key, string value);

    IAsyncEnumerable<(string Value, double Score)> GetAllSortedSetData(string key, long pageSize, Order order);
    Task<double> CreateOrIncrementSortedSetDataAsync(string key, string value, int score);
    Task<bool> UpdateSortedSetDataAsync(string key, string oldValue, string newValue);
    Task<bool> DeleteSortedSetDataAsync(string key, string value);

    Task<long> ListRightPushAsync(string key, string value);
    Task<string[]> ListRangeAsync(string key, long start = 0, long stop = -1);
    Task ListTrimAsync(string key, long start, long stop);

    IAsyncEnumerable<(string Key, List<HashEntry> Entries)> GetAllHashDataAsync(string keyPattern);
    Task<List<HashEntry>?> GetHashDataAsync(string key);
    Task<List<HashEntry>?> CreateHashDataAsync<T>(string key, T value);
    Task<List<HashEntry>?> UpdateHashDataAsync<T>(string key, T value);
    Task<bool> DeleteHashDataAsync(string key);

    Task<bool> SetKeyExpiryTimeAsync(string key, TimeSpan expiryTime);
}