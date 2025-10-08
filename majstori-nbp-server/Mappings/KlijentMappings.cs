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
            GetKlijentDTO klijent = new();
            string id = entry.Key.Substring("klijent:".Length);
            klijent.Id = Guid.Parse(id);

            var valueEntries = entry.Value;

            klijent.Ime = valueEntries.FirstOrDefault(x => x.Name == "ime").Value!;
            klijent.Prezime = valueEntries.FirstOrDefault(x => x.Name == "prezime").Value!;
            klijent.BrojTelefona = valueEntries.FirstOrDefault(x => x.Name == "brojtelefona").Value!;
            klijent.Email = valueEntries.FirstOrDefault(x => x.Name == "email").Value!;

            klijenti.Add(klijent);
        }

        return klijenti;
    }

    public static GetKlijentDTO MapToGetKlijentDTO(this List<HashEntry> entries, string id)
    {
        return new GetKlijentDTO
        {
            Id = Guid.Parse(id),
            Ime = entries[0].Value.ToString(),
            Prezime = entries[1].Value.ToString(),
            BrojTelefona = entries[2].Value.ToString(),
            Email = entries[3].Value.ToString()
        };
    }
}