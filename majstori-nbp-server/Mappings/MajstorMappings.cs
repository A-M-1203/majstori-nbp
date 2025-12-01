using majstori_nbp_server.DTOs.AuthDTOs;
using majstori_nbp_server.DTOs.MajstorDTOs;
using StackExchange.Redis;

namespace majstori_nbp_server.Mappings;

public static class MajstorMappings
{
    public static List<GetMajstorDTO> MapToListOfGetMajstorDTO(this Dictionary<string, List<HashEntry>> data)
    {
        List<GetMajstorDTO> majstori = new(data.Count);
        foreach (var entry in data)
        {
            string id = entry.Key.Substring("majstor:".Length);
            var valueEntries = entry.Value;
            var majstor = new GetMajstorDTO
            {
                Id = Guid.Parse(id),
                Ime = valueEntries.First(x => x.Name == "ime").Value!,
                Prezime = valueEntries.First(x => x.Name == "prezime").Value!,
                Email = valueEntries.First(x => x.Name == "email").Value!,
                PasswordHash = valueEntries.First(x => x.Name == "passwordhash").Value!,
                Lokacija = valueEntries.First(x => x.Name == "lokacija").Value!,
                BrojTelefona = valueEntries.First(x => x.Name == "brojtelefona").Value!,
                Slika = valueEntries.First(x => x.Name == "slika").Value
            };

            majstori.Add(majstor);
        }

        return majstori;
    }

    public static GetMajstorDTO MapToGetMajstorDTO(this List<HashEntry> entries, string id)
    {
        return new GetMajstorDTO
        {
            Id = Guid.Parse(id),
            Ime = entries[1].Value.ToString(),
            Prezime = entries[4].Value.ToString(),
            Email = entries[6].Value.ToString(),
            PasswordHash = entries[2].Value.ToString(),
            Lokacija = entries[0].Value.ToString(),
            BrojTelefona = entries[3].Value.ToString(),
            Slika = entries[5].Value.ToString()
        };
    }

    public static CreateMajstorDTO MapToCreateMajstorDTO(this RegisterMajstorDTO majstorDTO, string passwordHash)
    {
        return new CreateMajstorDTO
        {
            Ime = majstorDTO.Ime,
            Prezime = majstorDTO.Prezime,
            Email = majstorDTO.Email,
            PasswordHash = passwordHash,
            BrojTelefona = majstorDTO.BrojTelefona,
            Lokacija = majstorDTO.Lokacija,
            Slika = majstorDTO.Slika
        };
    }

    public static UserDTO MapToUserDTO(this RegisterMajstorDTO majstorDTO)
    {
        return new UserDTO
        {
            Email = majstorDTO.Email,
            Password = majstorDTO.Password
        };
    }
}