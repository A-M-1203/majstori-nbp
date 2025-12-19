namespace majstori_nbp_server.DTOs.KlijentDTOs;

public class UpdateKlijentDTO
{
    public  string ime { get; set; }

    
    public string prezime { get; set; }

    
    public  string broj { get; set; }

    
    
  
    public  string adresa { get; set; }
    
  
    public  string lokacija { get; set; }
    
    
    public IFormFile? image { get; set; }
}