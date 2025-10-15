using StackExchange.Redis;

namespace majstori_nbp_server.Services;

public interface ICacheService
{
    Task<List<string?>> GetAllData(string keyPattern);
    Task<string?> GetDataAsync(string key);

    IEnumerable<string> GetAllSetData(string key);
    Task<bool> GetSetDataAsync(string key, string value);

    // IEnumerable<string> GetAllSortedSetData(string key);

    Task<Dictionary<string, List<HashEntry>>> GetAllHashDataAsync(string keyPattern);
    Task<List<HashEntry>> GetHashDataAsync(string key);

    Task<bool> CreateDataAsync(string key, string value);
    Task<bool> CreateSetDataAsync(string key, string value);
    Task<double> CreateSortedSetDataAsync(string key, string value, int score);
    Task<List<HashEntry>> CreateHashDataAsync<T>(string key, T value);

    Task<List<HashEntry>> UpdateHashDataAsync<T>(string key, T value);

    Task<bool> SetKeyExpiryTimeAsync(string key, TimeSpan expiryTime);

    Task<bool> DeleteDataAsync(string key);
    Task<bool> DeleteSetDataAsync(string key, string value);
}