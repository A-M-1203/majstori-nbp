using System.Threading.Tasks;
using majstori_nbp_server.Services;
using StackExchange.Redis;

namespace majstori_nbp_server.Implementations;

public class RedisCacheService : ICacheService
{
    private IDatabase _redisDb;
    private IServer serverConfig;

    public RedisCacheService()
    {
        var connection = ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints =
            {
                { "redis-16631.c300.eu-central-1-1.ec2.redns.redis-cloud.com", 16631 }
            },
            User = "default",
            Password = "zFPbn1BTJ9xRlpjlnrenDF3Cutn2lyC9"
        });
        _redisDb = connection.GetDatabase();
        serverConfig = connection.GetServer("redis-16631.c300.eu-central-1-1.ec2.redns.redis-cloud.com", 16631);
    }

    public async Task<List<string?>> GetAllData(string keyPattern)
    {
        List<string?> values = new();
        await foreach (var key in serverConfig.KeysAsync(pattern: keyPattern))
        {
            var value = await _redisDb.StringGetAsync(key);
            values.Add(value);
        }

        return values;
    }

    public async Task<string?> GetDataAsync(string key)
    {
        return await _redisDb.StringGetAsync(key);
    }

    public IEnumerable<string> GetAllSetData(string key)
    {
        var members = _redisDb.SetScan(key);
        foreach (var member in members)
        {
            yield return member.ToString();
        }
    }

    public async Task<bool> GetSetDataAsync(string key, string value)
    {
        return await _redisDb.SetContainsAsync(key, value);
    }

    // public async Task<IEnumerable<string>> GetAllSortedSetData(string key)
    // {
    //     await _redisDb.SortedSetRangeByRankWithScoresAsync(key, order: Order.Ascending);
    // }



    public async Task<Dictionary<string, List<HashEntry>>> GetAllHashDataAsync(string keyPattern)
    {
        Dictionary<string, List<HashEntry>> entries = new();
        await foreach (var key in serverConfig.KeysAsync(pattern: keyPattern))
        {
            var entry = await _redisDb.HashGetAllAsync(key);
            entries.Add(key!, entry.ToList());
        }

        return entries;
    }

    public async Task<List<HashEntry>> GetHashDataAsync(string key)
    {
        var entries = await _redisDb.HashGetAllAsync(key);
        return entries.ToList();
    }

    public async Task<bool> CreateDataAsync(string key, string value)
    {
        bool isSet = await _redisDb.StringSetAsync(key, value, when: When.NotExists);

        if (isSet)
        {
            Console.WriteLine($"Kljuc '{key}' je upisan sa vrednoscu '{value}'.");
        }
        else
        {
            Console.WriteLine($"Kljuc '{key}' nije upisan.");
        }

        return isSet;
    }

    public async Task<bool> CreateSetDataAsync(string key, string value)
    {
        return await _redisDb.SetAddAsync(key, value);
    }

    public async Task<double> CreateSortedSetDataAsync(string key, string value, int score)
    {
        return await _redisDb.SortedSetIncrementAsync(key, value, score);
    }

    public async Task<List<HashEntry>> CreateHashDataAsync<T>(string key, T value)
    {
        if (await _redisDb.KeyExistsAsync(key))
        {
            Console.WriteLine($"Kljuc {key} vec postoji.");
            return new();
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

        return new();
    }

    public async Task<List<HashEntry>> UpdateHashDataAsync<T>(string key, T value)
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

    public async Task<bool> SetKeyExpiryTimeAsync(string key, TimeSpan expiryTime)
    {
        bool isSet = await _redisDb.KeyExpireAsync(key, expiryTime);
        if (isSet)
        {
            Console.WriteLine($"Vreme vazenja kljuca '{key}' je postavljeno na {expiryTime.TotalSeconds} sekundi.");
        }
        else
        {
            Console.WriteLine($"Vreme vazenja kljuca '{key}' nije postavljeno.");
        }

        return isSet;
    }

    public async Task<bool> DeleteDataAsync(string key)
    {
        bool isRemoved = await _redisDb.KeyDeleteAsync(key);

        return isRemoved;
    }

    public async Task<bool> DeleteSetDataAsync(string key, string value)
    {
        return await _redisDb.SetRemoveAsync(key, value);
    }
}