using System.ComponentModel.DataAnnotations;

namespace majstori_nbp_server.Models;

public class Majstor
{
    public Guid Id { get; set; }

    [MinLength(2)]
    [MaxLength(30)]
    public required string Ime { get; set; }

    [MinLength(2)]
    [MaxLength(30)]
    public required string Prezime { get; set; }

    public required string BrojTelefona { get; set; }

    [EmailAddress]
    public required string Email { get; set; }
}