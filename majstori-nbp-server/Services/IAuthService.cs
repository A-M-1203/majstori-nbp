using majstori_nbp_server.DTOs.AuthDTOs;
using majstori_nbp_server.DTOs.KlijentDTOs;
using majstori_nbp_server.DTOs.MajstorDTOs;

namespace majstori_nbp_server.Services;

public interface IAuthService
{
    Task<GetMajstorDTO?> RegisterMajstorAsync(RegisterMajstorDTO registerRequest);
    Task<GetKlijentDTO?> RegisterKlijentAsync(RegisterKlijentDTO registerRequest);
    Task<LoginResponse<GetKlijentDTO>?> LoginKlijentAsync(UserDTO user, string userId);
    Task<LoginResponse<GetMajstorDTO>?> LoginMajstorAsync(UserDTO user, string userId);
}