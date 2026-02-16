namespace majstori_nbp_server.DTOs.MessageDTO;

public class CreateMessageDTO
{
    public string? _id { get; set; }
    public string sadrzaj { get; set; }
    public string korisnik { get; set; }
    public DateTime? datum { get; set; }
    public string chat { get; set; }
}