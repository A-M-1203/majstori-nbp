using majstori_nbp_server.DTOs.KategorijeDTOs;

namespace majstori_nbp_server.DTOs.MajstorDTOs;

public class UpdateMajstorDTO
{
    
    public  string ime { get; set; }

    
    public string prezime { get; set; }

    
    public  string broj { get; set; }

    
    
  
    public  string adresa { get; set; }
    
  
    public  string lokacija { get; set; }
    

    public List<string> podkategorija { get; set; }
    
    public IFormFile? image { get; set; }
}