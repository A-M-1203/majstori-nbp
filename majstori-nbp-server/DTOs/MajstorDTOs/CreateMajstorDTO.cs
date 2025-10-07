using System.ComponentModel.DataAnnotations;

namespace majstori_nbp_server.DTOs.MajstorDTOs;

public class CreateMajstorDTO
{
    [MinLength(2, ErrorMessage = "Minimalna duzina imena je 2 karaktera.")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina imena je 30 karaktera")]
    [Required(ErrorMessage = "Ime je obavezno polje.")]
    public required string Ime { get; set; }

    [MinLength(2, ErrorMessage = "Minimalna duzina prezimena je 2 karaktera.")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina prezimena je 30 karaktera")]
    [Required(ErrorMessage = "Prezime je obavezno polje.")]
    public required string Prezime { get; set; }

    [Phone(ErrorMessage = "Nevalidan broj telefona.")]
    [Required(ErrorMessage = "Broj telefona je obavezno polje.")]
    public required string BrojTelefona { get; set; }

    [EmailAddress(ErrorMessage = "Nevalidna email adresa.")]
    [Required(ErrorMessage = "Email adresa je obavezno polje.")]
    public required string Email { get; set; }
}