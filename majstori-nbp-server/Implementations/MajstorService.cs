using majstori_nbp_server.DTOs.MajstorDTOs;
using majstori_nbp_server.Mappings;
using majstori_nbp_server.Services;

namespace majstori_nbp_server.Implementations;

public class MajstorService : IMajstorService
{
    private readonly ICacheService _cacheService;
    private readonly IEmailService _emailService;
    private readonly string _majstorNamespace = "majstor:";

    public MajstorService(ICacheService cacheService, IEmailService emailService)
    {
        _cacheService = cacheService;
        _emailService = emailService;
    }

    public IEnumerable<string> GetAllEmails()
    {
        return _emailService.GetAllEmails(_majstorNamespace);
    }

    public async Task<string?> GetEmailAsync(string email)
    {
        return await _emailService.GetEmailAsync(email);
    }

    public async IAsyncEnumerable<GetMajstorDTO> GetAllAsync()
    {
        await foreach (var (key, entries) in _cacheService.GetAllHashDataAsync($"{_majstorNamespace}*"))
        {
            string id = key.Substring(_majstorNamespace.Length);
            var majstor = new GetMajstorDTO
            {
                Id = Guid.Parse(id),
                Ime = entries.First(x => x.Name == "ime").Value!,
                Prezime = entries.First(x => x.Name == "prezime").Value!,
                Email = entries.First(x => x.Name == "email").Value!,
                PasswordHash = entries.First(x => x.Name == "passwordhash").Value!,
                Lokacija = entries.First(x => x.Name == "lokacija").Value!,
                BrojTelefona = entries.First(x => x.Name == "brojtelefona").Value!,
                Slika = entries.First(x => x.Name == "slika").Value!
            };

            yield return majstor;

        }
    }

    public async Task<GetMajstorDTO?> GetByIdAsync(string id)
    {
        string key = _majstorNamespace + id;
        var entries = await _cacheService.GetHashDataAsync(key);
        if (entries is null)
        {
            return null;
        }

        return entries.MapToGetMajstorDTO(id);
    }

    public async Task<GetMajstorDTO?> CreateAsync(CreateMajstorDTO majstor)
    {
        string id = Guid.NewGuid().ToString();
        string key = _majstorNamespace + id;
        bool isCreated = await _emailService.CreateEmailAsync(_majstorNamespace, majstor.Email, key);
        if (isCreated is false)
        {
            return null;
        }

        var entries = await _cacheService.CreateHashDataAsync(key, majstor);
        if (entries is null)
        {
            return null;
        }

        return entries.MapToGetMajstorDTO(id);
    }

    public async Task<GetMajstorDTO?> UpdateAsync(string id, UpdateMajstorDTO majstor)
    {
        GetMajstorDTO? p = await GetByIdAsync(id);
        string key = _majstorNamespace + id;
        if (p != null && majstor.Email != null)
        {
            await _emailService.UpdateEmailAsync(_majstorNamespace, majstor.Email, p.Email, key);
        }

        var entries = await _cacheService.UpdateHashDataAsync(key, majstor);
        if (entries is null)
        {
            return null;
        }

        return entries.MapToGetMajstorDTO(id);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        GetMajstorDTO? p = await GetByIdAsync(id);
        if (p is not null)
        {
            await _emailService.DeleteEmailAsync(_majstorNamespace, p.Email);
        }
        string key = _majstorNamespace + id;
        bool isDeleted = await _cacheService.DeleteHashDataAsync(key);

        return isDeleted;
    }
}