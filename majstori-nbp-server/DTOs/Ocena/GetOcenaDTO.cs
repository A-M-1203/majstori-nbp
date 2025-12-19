using majstori_nbp_server.DTOs.MajstorDTOs;

namespace majstori_nbp_server.DTOs.Ocena;

public class GetOcenaDTO
{
    public string _id { get; set; }
    public string korisnik { get; set; }
    public GetMajstorDTO majstor { get; set; }
    public int? ocena { get; set; }
}