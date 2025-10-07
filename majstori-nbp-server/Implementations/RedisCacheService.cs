using majstori_nbp_server.Services;
using StackExchange.Redis;

namespace majstori_nbp_server.Implementations;

public class RedisCacheService : ICacheService
{
    private IDatabase _redisDb;

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
    }

    public async Task<string?> GetDataAsync(string key)
    {
        return await _redisDb.StringGetAsync(key);
    }

    public async Task<HashEntry[]> GetHashDataAsync(string key)
    {
        var entries = await _redisDb.HashGetAllAsync(key);
        return entries;
    }

    public async Task<bool> SetDataAsync(string key, string value)
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

    public async Task<bool> SetHashDataAsync<T>(string key, T value)
    {
        if (await _redisDb.KeyExistsAsync(key))
        {
            Console.WriteLine($"Kljuc {key} vec postoji.");
            return false;
        }

        var properties = typeof(T).GetProperties();
        var entries = new List<HashEntry>();

        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(value);
            if (propertyValue != null)
            {
                entries.Add(new HashEntry(property.Name.ToLower(), propertyValue.ToString()));
            }
        }

        await _redisDb.HashSetAsync(key, entries.ToArray());

        return true;
    }

    public async Task<bool> SetKeyExpiryTime(string key, TimeSpan expiryTime)
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

    public async Task<bool> RemoveData(string key)
    {
        bool isRemoved = await _redisDb.KeyDeleteAsync(key);
        if (isRemoved)
        {
            Console.WriteLine($"Kljuc '{key}' je obrisan.");
        }
        else
        {
            Console.WriteLine($"Kljuc '{key}' ne postoji.");
        }

        return isRemoved;
    }
}