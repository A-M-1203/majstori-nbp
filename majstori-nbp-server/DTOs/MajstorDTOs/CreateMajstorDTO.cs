using System.ComponentModel.DataAnnotations;

namespace majstori_nbp_server.DTOs.MajstorDTOs;

public class CreateMajstorDTO
{
    [MinLength(2, ErrorMessage = "Minimalna duzina imena je 2 karaktera.")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina imena je 30 karaktera")]
    [Required(ErrorMessage = "Ime je obavezno polje.")]
    public required string ime { get; set; }

    [MinLength(2, ErrorMessage = "Minimalna duzina prezimena je 2 karaktera.")]
    [MaxLength(30, ErrorMessage = "Maksimalna duzina prezimena je 30 karaktera")]
    [Required(ErrorMessage = "Prezime je obavezno polje.")]
    public required string prezime { get; set; }

    [MaxLength(20, ErrorMessage = "Maksimalna duzina broja telefona je 20 karaktera.")]
    [Phone(ErrorMessage = "Nevalidan broj telefona.")]
    [Required(ErrorMessage = "Broj telefona je obavezno polje.")]
    public required string broj { get; set; }

    [MaxLength(320, ErrorMessage = "Maksimalna duzina email adrese je 320 karaktera.")]
    [EmailAddress(ErrorMessage = "Nevalidna email adresa.")]
    [Required(ErrorMessage = "Email adresa je obavezno polje.")]
    public required string email { get; set; }
    
    [MaxLength(50, ErrorMessage = "Maksimalna duzina adrese je 50 karaktera.")]
    [Required(ErrorMessage = "Adresa je obavezno polje.")]
    public required string adresa { get; set; }
    
    [MaxLength(50, ErrorMessage = "Maksimalna duzina lokacije je 50 karaktera.")]
    [Required(ErrorMessage = "Lokacija je obavezno polje.")]
    public required string lokacija { get; set; }
    
    [MaxLength(20, ErrorMessage = "Maksimalna duzina passworda je 50 karaktera.")]
    [MinLength(8,ErrorMessage = "Minimalna duzina passworda je 8 karaktera.")]
    [Required(ErrorMessage = "Password je obavezno polje.")]
    public required string password { get; set; }

    public string[] podkategorija { get; set; }
}