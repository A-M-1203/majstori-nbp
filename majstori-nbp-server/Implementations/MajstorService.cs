using majstori_nbp_server.DTOs.KlijentDTOs;
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

    public async Task<IEnumerable<GetMajstorDTO>> GetAllAsync()
    {
        var data = await _cacheService.GetAllHashDataAsync("majstor:*");

        IEnumerable<GetMajstorDTO> majstori = data.MapToListOfGetMajstorDTO();

        return majstori;
    }

    public async Task<GetMajstorDTO?> GetByIdAsync(string id)
    {
        string key = "majstor:" + id;
        var entries = await _cacheService.GetHashDataAsync(key);
        GetMajstorDTO? majstor = null;
        if (entries.Count > 0)
        {
            majstor = entries.MapToGetMajstorDTO(id);
        }

        return majstor;
    }

    public async Task<GetMajstorDTO?> CreateAsync(CreateMajstorDTO majstor)
    {
        bool isCreated = await _emailService.CreateEmailAsync(majstor.Email);
        if (isCreated == false)
        {
            return null;
        }

        string id = Guid.NewGuid().ToString();
        string key = "majstor:" + id;
        var entries = await _cacheService.CreateHashDataAsync(key, majstor);
        GetMajstorDTO? noviMajstor = null;
        if (entries.Count > 0)
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
        if (entries.Count > 0)
        {
            azuriraniMajstor = entries.MapToGetMajstorDTO(id);
        }

        return azuriraniMajstor;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        GetMajstorDTO? p = await GetByIdAsync(id);
        if (p != null)
        {
            await _emailService.DeleteEmailAsync(p.Email);
        }
        string key = "majstor:" + id;
        bool isDeleted = await _cacheService.DeleteDataAsync(key);

        return isDeleted;
    }
}