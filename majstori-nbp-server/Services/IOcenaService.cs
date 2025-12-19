using majstori_nbp_server.DTOs.Ocena;

namespace majstori_nbp_server.Services;

public interface IOcenaService
{
    Task<bool> addOcena(string userId,string majstorId);
    Task<ListGetOcenaDTO> getOcena(string userId);
    
    Task<ListGetOcenaMajstorDTO>getOcenaByMajstorId(string majstorId);
    
    Task<bool>uploadOcena(string userId,string majstorId,int ocena);

    Task<double> averageOcena(string majstorId);
}