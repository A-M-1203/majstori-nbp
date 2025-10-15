namespace majstori_nbp_server.DTOs.PosaoDTOs;

public class GetPosaoDTO
{
    public Guid Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public int Score { get; set; }
}