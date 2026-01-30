using majstori_nbp_server.Services;
using StackExchange.Redis;

namespace majstori_nbp_server.Implementations;

public class RedisCacheService : ICacheService
{
    private IDatabase _redisDb;
    private IServer serverConfig;

    public RedisCacheService()
    {
        var connection = ConnectionMultiplexer.Connect("localhost:6379");
        _redisDb = connection.GetDatabase();
        serverConfig = connection.GetServer("localhost:6379");
    }



    // -------------- Redis String --------------

    public async IAsyncEnumerable<string> GetAllKeysAsync(string keyPattern)
    {
        await foreach (var key in serverConfig.KeysAsync(pattern: keyPattern))
        {
            yield return key.ToString();
        }
    }

    public async IAsyncEnumerable<string> GetAllStringsAsync(string keyPattern)
    {
        await foreach (var key in serverConfig.KeysAsync(pattern: keyPattern))
        {
            var keyType = await _redisDb.KeyTypeAsync(key);
            if (keyType != RedisType.String)
                continue;
            var value = await _redisDb.StringGetAsync(key);
            yield return value.ToString();
        }
    }

    public async Task<string?> GetStringAsync(string key)
    {
        var value = await _redisDb.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return null;
        return value.ToString();
    }

    public async Task<bool> CreateOrUpdateStringAsync(string key, string value)
    {
        return await _redisDb.StringSetAsync(key, value);
    }

    public async Task<bool> CreateOrUpdateStringWithExpiryAsync(string key, string value, TimeSpan expiry)
    {
        return await _redisDb.StringSetAsync(key, value, expiry);
    }

    public async Task<bool> DeleteStringAsync(string key)
    {
        return await _redisDb.KeyDeleteAsync(key);
    }



    // -------------- Redis Set --------------

    public async IAsyncEnumerable<string> GetAllSetDataAsync(string key)
    {
        var members = await _redisDb.SetMembersAsync(key);
        foreach (var member in members)
        {
            yield return member.ToString();
        }
    }

    public async Task<bool> SetDataExistsAsync(string key, string value)
    {
        return await _redisDb.SetContainsAsync(key, value);
    }

    public async Task<bool> CreateSetDataAsync(string key, string value)
    {
        return await _redisDb.SetAddAsync(key, value);
    }

    public async Task<bool> UpdateSetDataAsync(string key, string oldValue, string newValue)
    {
        bool isRemoved = await _redisDb.SetRemoveAsync(key, oldValue);
        if (isRemoved is false)
        {
            return false;
        }

        return await _redisDb.SetAddAsync(key, newValue);
    }

    public async Task<bool> DeleteSetDataAsync(string key, string value)
    {
        return await _redisDb.SetRemoveAsync(key, value);
    }



    // -------------- Redis Sorted Set --------------

    public async IAsyncEnumerable<double> GetAllSortedSetData(string key,
                                                             long pageSize = 100,
                                                             Order order = Order.Descending)
    {
        long start = 0;

        while (true)
        {
            var entries = await _redisDb.SortedSetRangeByRankWithScoresAsync(key, start,
                                                                            start + pageSize - 1,
                                                                            order);
            if (entries.Length == 0)
            {
                yield break;
            }

            foreach (var entry in entries)
            {
                yield return entry.Score;
            }

            start += pageSize;
        }
    }

    public async Task<double> CreateOrIncrementSortedSetDataAsync(string key, string value, int score)
    {
        return await _redisDb.SortedSetIncrementAsync(key, value, score);
    }

    public async Task<bool> UpdateSortedSetDataAsync(string key, string oldValue, string newValue)
    {
        var score = await _redisDb.SortedSetScoreAsync(key, oldValue);

        if (score is null)
        {
            return false;
        }

        bool isRemoved = await _redisDb.SortedSetRemoveAsync(key, oldValue);
        if (isRemoved is false)
        {
            return false;
        }

        return await _redisDb.SortedSetAddAsync(key, oldValue, score.Value);
    }

    public async Task<bool> DeleteSortedSetDataAsync(string key, string value)
    {
        return await _redisDb.SortedSetRemoveAsync(key, value);
    }



    // -------------- Redis Hash --------------

    public async IAsyncEnumerable<List<HashEntry>> GetAllHashDataAsync(string keyPattern)
    {
        await foreach (var key in serverConfig.KeysAsync(pattern: keyPattern))
        {
            var keyType = await _redisDb.KeyTypeAsync(key);
            if (keyType != RedisType.Hash)
                continue;

            var entries = await _redisDb.HashGetAllAsync(key);
            yield return entries.ToList();
        }
    }

    public async Task<List<HashEntry>?> GetHashDataAsync(string key)
    {
        var entries = await _redisDb.HashGetAllAsync(key);
        if (entries is null)
        {
            return null;
        }
        return entries.ToList();
    }

    public async Task<List<HashEntry>?> CreateOrUpdateHashDataAsync<T>(string key, T value)
    {
        var properties = typeof(T).GetProperties();
        var entries = new List<HashEntry>();

        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(value);
            entries.Add(new HashEntry(property.Name.ToLower(), propertyValue?.ToString() ?? ""));
        }

        if (entries.Count > 0)
        {
            await _redisDb.HashSetAsync(key, entries.ToArray());
            return await GetHashDataAsync(key);
        }

        return null;
    }

    public async Task<bool> DeleteHashDataAsync(string key)
    {
        return await DeleteStringAsync(key);
    }

    public async Task<bool> IsHashKeyAsync(string key)
    {
        var keyType = await _redisDb.KeyTypeAsync(key);
        return keyType == RedisType.Hash;
    }

    public async Task<bool> SetKeyExpiryTimeAsync(string key, TimeSpan expiryTime)
    {
        return await _redisDb.KeyExpireAsync(key, expiryTime);
    }
}