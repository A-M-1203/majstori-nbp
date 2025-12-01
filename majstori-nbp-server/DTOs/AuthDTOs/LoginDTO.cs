namespace majstori_nbp_server.DTOs.AuthDTOs;

public class LoginDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}