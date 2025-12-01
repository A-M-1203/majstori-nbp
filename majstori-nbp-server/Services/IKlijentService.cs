using majstori_nbp_server.DTOs.KlijentDTOs;

namespace majstori_nbp_server.Services;

public interface IKlijentService
{
    IEnumerable<string> GetAllEmails();
    Task<string?> GetEmailAsync(string email);
    Task<string?> GetUserId(string email);
    IAsyncEnumerable<GetKlijentDTO> GetAllAsync();
    Task<GetKlijentDTO?> GetByIdAsync(string id);
    Task<GetKlijentDTO?> CreateAsync(CreateKlijentDTO klijent);
    Task<GetKlijentDTO?> UpdateAsync(string id, UpdateKlijentDTO klijent);
    Task<bool> DeleteAsync(string id);
}