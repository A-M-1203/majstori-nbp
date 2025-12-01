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

    [EmailAddress(ErrorMessage = "Nevalidna email adresa.")]
    [MaxLength(250, ErrorMessage = "Maksimalna duzina email adrese je 250 karaktera")]
    [Required(ErrorMessage = "Email adresa je obavezno polje.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "PasswordHash je obavezno polje.")]
    public required string PasswordHash { get; set; }

    [MinLength(2, ErrorMessage = "Minimalna duzina lokacije je 2 karaktera.")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina lokacije je 30 karaktera")]
    [Required(ErrorMessage = "Lokacija je obavezno polje.")]
    public required string Lokacija { get; set; }

    [Phone(ErrorMessage = "Nevalidan broj telefona.")]
    [Required(ErrorMessage = "Broj telefona je obavezno polje.")]
    public required string BrojTelefona { get; set; }

    public string? Slika { get; set; }
}