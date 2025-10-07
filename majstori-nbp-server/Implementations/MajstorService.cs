using majstori_nbp_server.DTOs.MajstorDTOs;
using majstori_nbp_server.Mappings;
using majstori_nbp_server.Services;

namespace majstori_nbp_server.Implementations;

public class MajstorService : IMajstorService
{
    private readonly ICacheService _cacheService;
    public MajstorService(ICacheService cacheService)
    {
        _cacheService = cacheService;
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
        string key = "majstor:" + id;
        bool isDeleted = await _cacheService.DeleteDataAsync(key);

        return isDeleted;
    }
}