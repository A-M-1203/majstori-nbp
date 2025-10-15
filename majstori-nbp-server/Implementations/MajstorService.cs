using majstori_nbp_server.DTOs.MajstorDTOs;
using majstori_nbp_server.Mappings;
using majstori_nbp_server.Services;

namespace majstori_nbp_server.Implementations;

public class MajstorService : IMajstorService
{
    private readonly ICacheService _cacheService;
    private readonly IEmailService _emailService;

    public MajstorService(ICacheService cacheService, IEmailService emailService)
    {
        _cacheService = cacheService;
        _emailService = emailService;
    }

    public IEnumerable<string> GetAllEmails()
    {
        return _emailService.GetAllEmails();
    }

    public async IAsyncEnumerable<GetMajstorDTO> GetAllAsync()
    {
        await foreach (var (key, entries) in _cacheService.GetAllHashDataAsync("majstor:*"))
        {
            GetMajstorDTO majstor = new();
            string id = key.Substring("majstor:".Length);
            majstor.Id = Guid.Parse(id);

            majstor.Ime = entries.FirstOrDefault(x => x.Name == "ime").Value!;
            majstor.Prezime = entries.FirstOrDefault(x => x.Name == "prezime").Value!;
            majstor.BrojTelefona = entries.FirstOrDefault(x => x.Name == "brojtelefona").Value!;
            majstor.Email = entries.FirstOrDefault(x => x.Name == "email").Value!;

            yield return majstor;

        }
    }

    public async Task<GetMajstorDTO?> GetByIdAsync(string id)
    {
        string key = "majstor:" + id;
        var entries = await _cacheService.GetHashDataAsync(key);
        GetMajstorDTO? majstor = null;
        if (entries is not null)
        {
            majstor = entries.MapToGetMajstorDTO(id);
        }

        return majstor;
    }

    public async Task<GetMajstorDTO?> CreateAsync(CreateMajstorDTO majstor)
    {
        bool isCreated = await _emailService.CreateEmailAsync(majstor.Email);
        if (isCreated is false)
        {
            return null;
        }

        string id = Guid.NewGuid().ToString();
        string key = "majstor:" + id;
        var entries = await _cacheService.CreateHashDataAsync(key, majstor);
        GetMajstorDTO? noviMajstor = null;
        if (entries is not null)
        {
            noviMajstor = entries.MapToGetMajstorDTO(id);
        }

        return noviMajstor;
    }

    public async Task<GetMajstorDTO?> UpdateAsync(string id, UpdateMajstorDTO majstor)
    {
        GetMajstorDTO? p = await GetByIdAsync(id);
        if (p != null && majstor.Email != null)
        {
            await _emailService.UpdateEmailAsync(majstor.Email, p.Email);
        }

        string key = "majstor:" + id;
        var entries = await _cacheService.UpdateHashDataAsync(key, majstor);
        GetMajstorDTO? azuriraniMajstor = null;
        if (entries is not null)
        {
            azuriraniMajstor = entries.MapToGetMajstorDTO(id);
        }

        return azuriraniMajstor;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        GetMajstorDTO? p = await GetByIdAsync(id);
        if (p is not null)
        {
            await _emailService.DeleteEmailAsync(p.Email);
        }
        string key = "majstor:" + id;
        bool isDeleted = await _cacheService.DeleteHashDataAsync(key);

        return isDeleted;
    }
}