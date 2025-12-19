using majstori_nbp_server.DTOs.KlijentDTOs;

namespace majstori_nbp_server.DTOs.Ocena;

public class GetOcenaMajstorDTO
{
    public string _id { get; set; }
    public string majstor { get; set; }
    public GetKlijentDTO korisnik { get; set; }
}