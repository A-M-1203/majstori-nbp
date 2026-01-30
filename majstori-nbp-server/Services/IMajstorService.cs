using majstori_nbp_server.DTOs.MajstorDTOs;

namespace majstori_nbp_server.Services;

public interface IMajstorService
{
    Task<IEnumerable<string>> GetAllEmailsAsync();
    Task<string?> GetEmailAsync(string email);
    IAsyncEnumerable<GetMajstorDTO> GetAllAsync();
    Task<GetMajstorDTO?> GetByIdAsync(string id);
    Task<GetMajstorDTO?> CreateAsync(CreateMajstorDTO majstor);
    Task<GetMajstorDTO?> UpdateAsync(string id, UpdateMajstorDTO majstor);
    Task<bool> DeleteAsync(string id);
}