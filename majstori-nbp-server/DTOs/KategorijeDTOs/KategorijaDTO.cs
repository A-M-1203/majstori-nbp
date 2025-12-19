namespace majstori_nbp_server.DTOs.KategorijeDTOs;

public class KategorijaDTO
{
    public required string _id{get;set;}
    public required string  naziv { get; set; }

    public List<PodkategorijaDTO> podkategorije { get; set; } = new();
}