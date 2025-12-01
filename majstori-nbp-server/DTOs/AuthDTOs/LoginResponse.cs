namespace majstori_nbp_server.DTOs.AuthDTOs;

public class LoginResponse<T> where T : class
{
    public required T User { get; set; }
    public required string Token { get; set; }
}