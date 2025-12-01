namespace majstori_nbp_server.DTOs.AuthDTOs;

public class RegisterKlijentDTO
{
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}