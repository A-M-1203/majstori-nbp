namespace majstori_nbp_server.DTOs.MessageDTO;

public class CreateMessageDTO
{
    public string? _id { get; set; }

    public string sadrzaj { get; set; } = null!;

    public string? korisnik { get; set; }   // <-- BITNO: nullable

    public DateTime? datum { get; set; }

    public string chat { get; set; } = null!;
}
