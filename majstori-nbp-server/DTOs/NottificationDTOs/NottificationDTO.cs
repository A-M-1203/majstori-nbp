namespace majstori_nbp_server.DTOs.NottificationDTOs;

public class NottificationDTO
{
    public string id { get; set; }
    public string text { get; set; }
    public DateTime time { get; set; }
    public string avatarUrl { get; set; }

    public string korisnik { get; set; }
}