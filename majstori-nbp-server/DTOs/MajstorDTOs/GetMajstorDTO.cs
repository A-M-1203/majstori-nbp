using majstori_nbp_server.DTOs.KategorijeDTOs;

namespace majstori_nbp_server.DTOs.MajstorDTOs;

public class GetMajstorDTO
{
    public string _id { get; set; }
    
    public  string ime { get; set; }

    
    public string prezime { get; set; }

    
    public  string broj { get; set; }

   
    public  string email { get; set; }
    
  
    public  string adresa { get; set; }
    
  
    public  string lokacija { get; set; }

    public string profilePicture { get; set; }

    public List<PodkategorijaDTO> podkategorija { get; set; }

    
}