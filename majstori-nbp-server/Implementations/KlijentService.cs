using majstori_nbp_server.DTOs.KlijentDTOs;
using majstori_nbp_server.Mappings;
using majstori_nbp_server.Services;

namespace majstori_nbp_server.Implementations;

public class KlijentService : IKlijentService
{
    private readonly ICacheService _cacheService;
    private readonly IEmailService _emailService;

    public KlijentService(ICacheService cacheService, IEmailService emailService)
    {
        _cacheService = cacheService;
        _emailService = emailService;
    }

    public IEnumerable<string> GetAllEmails()
    {
        return _emailService.GetAllEmails();
    }

    public async IAsyncEnumerable<GetKlijentDTO> GetAllAsync()
    {
        await foreach (var (key, entries) in _cacheService.GetAllHashDataAsync("klijent:*"))
        {
            GetKlijentDTO klijent = new();
            string id = key.Substring("klijent:".Length);
            klijent.Id = Guid.Parse(id);

            klijent.Ime = entries.FirstOrDefault(x => x.Name == "ime").Value!;
            klijent.Prezime = entries.FirstOrDefault(x => x.Name == "prezime").Value!;
            klijent.BrojTelefona = entries.FirstOrDefault(x => x.Name == "brojtelefona").Value!;
            klijent.Email = entries.FirstOrDefault(x => x.Name == "email").Value!;

            yield return klijent;

        }
    }

    public async Task<GetKlijentDTO?> GetByIdAsync(string id)
    {
        string key = "klijent:" + id;
        var entries = await _cacheService.GetHashDataAsync(key);
        GetKlijentDTO? klijent = null;
        if (entries is not null)
        {
            klijent = entries.MapToGetKlijentDTO(id);
        }

        return klijent;
    }

    public async Task<GetKlijentDTO?> CreateAsync(CreateKlijentDTO klijent)
    {
        bool isCreated = await _emailService.CreateEmailAsync(klijent.Email);
        if (isCreated is false)
        {
            return null;
        }

        string id = Guid.NewGuid().ToString();
        string key = "klijent:" + id;
        var entries = await _cacheService.CreateHashDataAsync(key, klijent);
        GetKlijentDTO? noviKlijent = null;
        if (entries is not null)
        {
            noviKlijent = entries.MapToGetKlijentDTO(id);
        }

        return noviKlijent;
    }

    public async Task<GetKlijentDTO?> UpdateAsync(string id, UpdateKlijentDTO klijent)
    {
        GetKlijentDTO? p = await GetByIdAsync(id);
        if (p != null && klijent.Email != null)
        {
            await _emailService.UpdateEmailAsync(klijent.Email, p.Email);
        }

        string key = "klijent:" + id;
        var entries = await _cacheService.UpdateHashDataAsync(key, klijent);
        GetKlijentDTO? azuriraniKlijent = null;
        if (entries is not null)
        {
            azuriraniKlijent = entries.MapToGetKlijentDTO(id);
        }

        return azuriraniKlijent;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        GetKlijentDTO? p = await GetByIdAsync(id);
        if (p is not null)
        {
            await _emailService.DeleteEmailAsync(p.Email);
        }
        string key = "klijent:" + id;
        bool isDeleted = await _cacheService.DeleteHashDataAsync(key);

        return isDeleted;
    }
}