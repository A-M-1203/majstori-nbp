using System.ComponentModel.DataAnnotations;

namespace majstori_nbp_server.DTOs.KlijentDTOs;

public class CreateKlijentDTO
{
    [MinLength(2, ErrorMessage = "Minimalna duzina imena je 2 karaktera.")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina imena je 30 karaktera")]
    [Required(ErrorMessage = "Ime je obavezno polje.")]
    public required string Ime { get; set; }

    [MinLength(2, ErrorMessage = "Minimalna duzina prezimena je 2 karaktera.")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina prezimena je 30 karaktera")]
    [Required(ErrorMessage = "Prezime je obavezno polje.")]
    public required string Prezime { get; set; }

    [MaxLength(20, ErrorMessage = "Maksimalna duzina broja telefona je 20 karaktera.")]
    [Phone(ErrorMessage = "Nevalidan broj telefona.")]
    [Required(ErrorMessage = "Broj telefona je obavezno polje.")]
    public required string BrojTelefona { get; set; }

    [MaxLength(320, ErrorMessage = "Maksimalna duzina email adrese je 320 karaktera.")]
    [EmailAddress(ErrorMessage = "Nevalidna email adresa.")]
    [Required(ErrorMessage = "Email adresa je obavezno polje.")]
    public required string Email { get; set; }
}