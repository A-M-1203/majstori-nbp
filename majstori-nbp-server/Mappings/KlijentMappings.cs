using majstori_nbp_server.DTOs.AuthDTOs;
using majstori_nbp_server.DTOs.KlijentDTOs;
using StackExchange.Redis;

namespace majstori_nbp_server.Mappings;

public static class KlijentMappings
{
    public static List<GetKlijentDTO> MapToListOfGetKlijentDTO(this Dictionary<string, List<HashEntry>> data)
    {
        List<GetKlijentDTO> klijenti = new(data.Count);
        foreach (var entry in data)
        {
            string id = entry.Key.Substring("klijent:".Length);
            var valueEntries = entry.Value;
            var klijent = new GetKlijentDTO
            {
                Id = Guid.Parse(id),
                Ime = valueEntries.First(x => x.Name == "ime").Value!,
                Prezime = valueEntries.First(x => x.Name == "prezime").Value!,
                Email = valueEntries.First(x => x.Name == "email").Value!,
                PasswordHash = valueEntries.First(x => x.Name == "passwordhash").Value!
            };

            klijenti.Add(klijent);
        }

        return klijenti;
    }

    public static GetKlijentDTO MapToGetKlijentDTO(this List<HashEntry> entries, string id)
    {
        return new GetKlijentDTO
        {
            Id = Guid.Parse(id),
            Ime = entries[3].Value.ToString(),
            Prezime = entries[0].Value.ToString(),
            Email = entries[1].Value.ToString(),
            PasswordHash = entries[2].Value.ToString()
        };
    }

    public static CreateKlijentDTO MapToCreateKlijentDTO(this RegisterKlijentDTO klijentDTO, string passwordHash)
    {
        return new CreateKlijentDTO
        {
            Ime = klijentDTO.Ime,
            Prezime = klijentDTO.Prezime,
            PasswordHash = passwordHash,
            Email = klijentDTO.Email
        };
    }

    public static UserDTO MapToUserDTO(this RegisterKlijentDTO klijentDTO)
    {
        return new UserDTO
        {
            Email = klijentDTO.Email,
            Password = klijentDTO.Password
        };
    }
}