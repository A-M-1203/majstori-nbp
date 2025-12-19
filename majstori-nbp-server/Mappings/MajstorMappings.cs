using majstori_nbp_server.DTOs.MajstorDTOs;
using StackExchange.Redis;

namespace majstori_nbp_server.Mappings;

public static class MajstorMappings
{
    /*public static List<GetMajstorDTO> MapToListOfGetMajstorDTO(this Dictionary<string, List<HashEntry>> data)
    {
        List<GetMajstorDTO> majstori = new(data.Count);
        foreach (var entry in data)
        {
            GetMajstorDTO majstor = new();
            string id = entry.Key.Substring("majstor:".Length);
            majstor.Id = Guid.Parse(id);

            var valueEntries = entry.Value;

            majstor.Ime = valueEntries.FirstOrDefault(x => x.Name == "ime").Value!;
            majstor.Prezime = valueEntries.FirstOrDefault(x => x.Name == "prezime").Value!;
            majstor.BrojTelefona = valueEntries.FirstOrDefault(x => x.Name == "brojtelefona").Value!;
            majstor.Email = valueEntries.FirstOrDefault(x => x.Name == "email").Value!;

            majstori.Add(majstor);
        }

        return majstori;
    }

    public static GetMajstorDTO MapToGetMajstorDTO(this List<HashEntry> entries, string id)
    {
        return new GetMajstorDTO
        {
            Id = Guid.Parse(id),
            Ime = entries[0].Value.ToString(),
            Prezime = entries[1].Value.ToString(),
            BrojTelefona = entries[2].Value.ToString(),
            Email = entries[3].Value.ToString()
        };
    }*/
}