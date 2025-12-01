using majstori_nbp_server.DTOs.KlijentDTOs;
using majstori_nbp_server.Mappings;
using majstori_nbp_server.Services;

namespace majstori_nbp_server.Implementations;

public class KlijentService : IKlijentService
{
    private readonly ICacheService _cacheService;
    private readonly IEmailService _emailService;
    private readonly string _klijentNamespace = "klijent:";

    public KlijentService(ICacheService cacheService, IEmailService emailService)
    {
        _cacheService = cacheService;
        _emailService = emailService;
    }

    public IEnumerable<string> GetAllEmails()
    {
        return _emailService.GetAllEmails(_klijentNamespace);
    }

    public async Task<string?> GetEmailAsync(string email)
    {
        return await _emailService.GetEmailAsync(email);
    }

    public async Task<string?> GetUserId(string email)
    {
        return await _cacheService.GetDataAsync(email);
    }

    public async IAsyncEnumerable<GetKlijentDTO> GetAllAsync()
    {
        await foreach (var (key, entries) in _cacheService.GetAllHashDataAsync($"{_klijentNamespace}*"))
        {
            string id = key.Substring(_klijentNamespace.Length);
            var klijent = new GetKlijentDTO
            {
                Id = Guid.Parse(id),
                Ime = entries.First(x => x.Name == "ime").Value!,
                Prezime = entries.First(x => x.Name == "prezime").Value!,
                Email = entries.First(x => x.Name == "email").Value!,
                PasswordHash = entries.First(x => x.Name == "passwordhash").Value!
            };

            yield return klijent;

        }
    }

    public async Task<GetKlijentDTO?> GetByIdAsync(string id)
    {
        string key = _klijentNamespace + id;
        var entries = await _cacheService.GetHashDataAsync(key);
        if (entries is null)
        {
            return null;
        }

        return entries.MapToGetKlijentDTO(id);
    }

    public async Task<GetKlijentDTO?> CreateAsync(CreateKlijentDTO klijent)
    {
        string id = Guid.NewGuid().ToString();
        string key = _klijentNamespace + id;
        bool isCreated = await _emailService.CreateEmailAsync(_klijentNamespace, klijent.Email, key);
        if (isCreated is false)
        {
            return null;
        }

        var entries = await _cacheService.CreateHashDataAsync(key, klijent);
        if (entries is null)
        {
            return null;
        }

        await _cacheService.CreateDataAsync(klijent.Email, key);

        return entries.MapToGetKlijentDTO(id);
    }

    public async Task<GetKlijentDTO?> UpdateAsync(string id, UpdateKlijentDTO klijent)
    {
        string key = _klijentNamespace + id;
        GetKlijentDTO? p = await GetByIdAsync(id);
        if (p != null && klijent.Email != null)
        {
            await _emailService.UpdateEmailAsync(_klijentNamespace, klijent.Email, p.Email, key);
        }

        var entries = await _cacheService.UpdateHashDataAsync(key, klijent);
        if (entries is null)
        {
            return null;
        }

        return entries.MapToGetKlijentDTO(id);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        GetKlijentDTO? p = await GetByIdAsync(id);
        if (p is not null)
        {
            await _emailService.DeleteEmailAsync(_klijentNamespace, p.Email);
        }
        string key = _klijentNamespace + id;
        bool isDeleted = await _cacheService.DeleteHashDataAsync(key);

        return isDeleted;
    }
}