using majstori_nbp_server.DTOs.MajstorDTOs;

namespace majstori_nbp_server.Services;

public interface IMajstorService
{
    IEnumerable<string> GetAllEmails();
    IAsyncEnumerable<GetMajstorDTO> GetAllAsync();
    Task<GetMajstorDTO?> GetByIdAsync(string id);
    Task<bool> CreateAsync(CreateMajstorDTO majstor);
    Task<GetMajstorDTO?> UpdateAsync( string id,UpdateMajstorDTO majstor);
    Task<bool> DeleteAsync(string id);
    Task<string>SignIn(string email, string password);
    Task<List<GetMajstorDTO>>getMajstorsByKategorijaId(string? kategorijaId,string? podkategorijaId);
}