using System.ComponentModel.DataAnnotations;

namespace majstori_nbp_server.DTOs;

public class LoginDTO
{
    [MinLength(2, ErrorMessage = "Minimalna duzina naziva posla je 2 karaktera.")]
    [MaxLength(50, ErrorMessage = "Maksimalna duzina naziva posla je 50 karaktera.")]
    [Required(ErrorMessage = "Naziv email je obavezno polje.")]
    public string email { get; set; }
    [MinLength(2, ErrorMessage = "Minimalna duzina naziva posla je 2 karaktera.")]
    [MaxLength(50, ErrorMessage = "Maksimalna duzina naziva posla je 50 karaktera.")]
    [Required(ErrorMessage = "Naziv password je obavezno polje.")]
    public string password { get; set; }
}