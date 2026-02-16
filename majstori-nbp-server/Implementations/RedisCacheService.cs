using majstori_nbp_server.Services;
using Neo4j.Driver;
using StackExchange.Redis;
using System.Linq;
namespace majstori_nbp_server.Implementations;

public class RedisCacheService : ICacheService
{

    private readonly IDatabase _redisDb;
    private readonly IConnectionMultiplexer _redis;
    private readonly IServer _server;

//novi konstruktor
    public RedisCacheService(IDatabase redisDb, IConnectionMultiplexer redis)
    {
        _redisDb = redisDb;
        _redis = redis;

        var endpoint = _redis.GetEndPoints().First();
        _server = _redis.GetServer(endpoint);
    }
    
    //dodato
    public async Task<long> ListRightPushAsync(string key, string value)
    => await _redisDb.ListRightPushAsync(key, value);

    public async Task<string[]> ListRangeAsync(string key, long start = 0, long stop = -1)
    {
        var vals = await _redisDb.ListRangeAsync(key, start, stop);
        return vals.Select(v => v.ToString()).ToArray();
    }

    public Task ListTrimAsync(string key, long start, long stop)
        => _redisDb.ListTrimAsync(key, start, stop);


    public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
    {
        return await _redisDb.StringSetAsync(key, value, expiry);
    }

    public async Task<string?> GetStringAsync(string key)
    {
        var v = await _redisDb.StringGetAsync(key);
        return v.HasValue ? v.ToString() : null;
    }

  

    public async IAsyncEnumerable<string> GetAllDataAsync(string keyPattern)
    {
        await foreach (var key in _server.KeysAsync(pattern: keyPattern))
        {
            yield return key.ToString();
        }
    }
    public async Task<string?> GetDataAsync(string key)
    {
        var v = await _redisDb.StringGetAsync(key);
        return v.HasValue ? v.ToString() : null;
    }
    // public async Task<string?> GetDataAsync(string key)
    // {
    //     return (await _redisDb.StringGetAsync(key)).ToString();
    // }

    public async Task<bool> CreateDataAsync(string key, string value)
    {
        bool isCreated = await _redisDb.StringSetAsync(key, value, when: When.NotExists);
        return isCreated;
    }

    public async Task<bool> CreateDataWithExpiryAsync(string key, string value, TimeSpan expiry)
    {
        bool isCreated = await _redisDb.StringSetAsync(key, value, expiry, when: When.NotExists);
        return isCreated;
    }

    public async Task<bool> UpdateDataAsync(string key, string newValue)
    {
        bool isUpdated = await _redisDb.StringSetAsync(key, newValue, when: When.Exists);
        return isUpdated;
    }

    public async Task<bool> UpdateDataWithExpiryAsync(string key, string newValue, TimeSpan expiry)
    {
        bool isUpdated = await _redisDb.StringSetAsync(key, newValue, expiry, when: When.Exists);
        return isUpdated;
    }

    public async Task<bool> DeleteDataAsync(string key)
    {
        bool isRemoved = await _redisDb.KeyDeleteAsync(key);
        return isRemoved;
    }



    public IEnumerable<string> GetAllSetData(string key)
    {
        var members = _redisDb.SetScan(key);
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

        bool isUpdated = await _redisDb.SetAddAsync(key, newValue);
        return isUpdated;
    }

    public async Task<bool> DeleteSetDataAsync(string key, string value)
    {
        return await _redisDb.SetRemoveAsync(key, value);
    }



    public async IAsyncEnumerable<(string Value, double Score)> GetAllSortedSetData(string key,
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
                yield return (entry.Element.ToString(), entry.Score);
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

        bool isUpdated = await _redisDb.SortedSetAddAsync(key, oldValue, score.Value);
        return isUpdated;
    }

    public async Task<bool> DeleteSortedSetDataAsync(string key, string value)
    {
        bool isRemoved = await _redisDb.SortedSetRemoveAsync(key, value);
        return isRemoved;
    }



    public async IAsyncEnumerable<(string Key, List<HashEntry> Entries)> GetAllHashDataAsync(string keyPattern)
    {
        await foreach (var key in _server.KeysAsync(pattern: keyPattern))
        {
            var entries = await _redisDb.HashGetAllAsync(key);
            yield return (key!, entries.ToList());
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

    public async Task<List<HashEntry>?> CreateHashDataAsync<T>(string key, T value)
    {
        if (await _redisDb.KeyExistsAsync(key))
        {
            return null;
        }

        var properties = typeof(T).GetProperties();
        var entries = new List<HashEntry>();

        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(value);
            if (propertyValue != null)
            {
                string stringValue = propertyValue.ToString()!;
                entries.Add(new HashEntry(property.Name.ToLower(), stringValue));
            }
        }

        if (entries.Count > 0)
        {
            await _redisDb.HashSetAsync(key, entries.ToArray());
            return await GetHashDataAsync(key);
        }

        return null;
    }

    public async Task<List<HashEntry>?> UpdateHashDataAsync<T>(string key, T value)
    {
        bool exists = await _redisDb.KeyExistsAsync(key);
        var entries = new List<HashEntry>();
        if (exists)
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(value);
                if (propertyValue != null)
                {
                    string stringValue = propertyValue.ToString()!;
                    entries.Add(new HashEntry(property.Name.ToLower(), stringValue));
                }
            }

            if (entries.Count > 0)
            {
                await _redisDb.HashSetAsync(key, entries.ToArray());
                return await GetHashDataAsync(key);
            }
        }

        return entries;
    }

    public async Task<bool> DeleteHashDataAsync(string key)
    {
        bool isRemoved = await DeleteDataAsync(key);
        return isRemoved;
    }



    public async Task<bool> SetKeyExpiryTimeAsync(string key, TimeSpan expiryTime)
    {
        bool isSet = await _redisDb.KeyExpireAsync(key, expiryTime);
        if (isSet is true)
        {
            Console.WriteLine($"Vreme vazenja kljuca '{key}' je postavljeno na {expiryTime.TotalSeconds} sekundi.");
        }
        else
        {
            Console.WriteLine($"Vreme vazenja kljuca '{key}' nije postavljeno.");
        }

        return isSet;
    }
}