namespace majstori_nbp_server.DTOs.MajstorDTOs;

public class GetMajstorDTO
{
    public Guid Id { get; set; }
    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public string BrojTelefona { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}