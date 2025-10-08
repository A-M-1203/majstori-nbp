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

    public async Task<IEnumerable<GetKlijentDTO>> GetAllAsync()
    {
        var data = await _cacheService.GetAllHashDataAsync("klijent:*");

        IEnumerable<GetKlijentDTO> klijenti = data.MapToListOfGetKlijentDTO();

        return klijenti;
    }

    public async Task<GetKlijentDTO?> GetByIdAsync(string id)
    {
        string key = "klijent:" + id;
        var entries = await _cacheService.GetHashDataAsync(key);
        GetKlijentDTO? klijent = null;
        if (entries.Count > 0)
        {
            klijent = entries.MapToGetKlijentDTO(id);
        }

        return klijent;
    }

    public async Task<GetKlijentDTO?> CreateAsync(CreateKlijentDTO klijent)
    {
        bool isCreated = await _emailService.CreateEmailAsync(klijent.Email);
        if (isCreated == false)
        {
            return null;
        }

        string id = Guid.NewGuid().ToString();
        string key = "klijent:" + id;
        var entries = await _cacheService.CreateHashDataAsync(key, klijent);
        GetKlijentDTO? noviKlijent = null;
        if (entries.Count > 0)
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
        if (entries.Count > 0)
        {
            azuriraniKlijent = entries.MapToGetKlijentDTO(id);
        }

        return azuriraniKlijent;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        GetKlijentDTO? p = await GetByIdAsync(id);
        if (p != null)
        {
            await _emailService.DeleteEmailAsync(p.Email);
        }
        string key = "klijent:" + id;
        bool isDeleted = await _cacheService.DeleteDataAsync(key);

        return isDeleted;
    }
}