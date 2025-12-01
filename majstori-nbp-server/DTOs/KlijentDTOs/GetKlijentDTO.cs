namespace majstori_nbp_server.DTOs.KlijentDTOs;

public class GetKlijentDTO
{
    public Guid Id { get; set; }
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}