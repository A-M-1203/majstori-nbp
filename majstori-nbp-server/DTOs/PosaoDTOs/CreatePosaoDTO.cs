using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace majstori_nbp_server.DTOs.PosaoDTOs;

public class CreatePosaoDTO
{
    [MinLength(2, ErrorMessage = "Minimalna duzina naziva posla je 2 karaktera.")]
    [MaxLength(50, ErrorMessage = "Maksimalna duzina naziva posla je 50 karaktera.")]
    [Required(ErrorMessage = "Naziv posla je obavezno polje.")]
    public required string Naziv { get; set; }

    [DefaultValue(0)]
    [Required(ErrorMessage = "Score je obavezno polje.")]
    public required int Score { get; set; } = 0;
}