using majstori_nbp_server.DTOs.KategorijeDTOs;

namespace majstori_nbp_server.Services;

public interface IKategorijaService
{
    Task<KategorijeDTO> GetKategorijas();
}