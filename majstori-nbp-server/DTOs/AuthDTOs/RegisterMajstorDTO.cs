namespace majstori_nbp_server.DTOs.AuthDTOs;

public class RegisterMajstorDTO
{
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Lokacija { get; set; }
    public required string BrojTelefona { get; set; }
    public string? Slika { get; set; }
}