using majstori_nbp_server.DTOs.KategorijeDTOs;
using majstori_nbp_server.DTOs.KlijentDTOs;
using majstori_nbp_server.DTOs.MajstorDTOs;
using majstori_nbp_server.DTOs.Ocena;
using majstori_nbp_server.Services;
using Neo4j.Driver;
using System.Text.Json;
using majstori_nbp_server.DTOs.NottificationDTOs;

namespace majstori_nbp_server.Implementations;

public class OcenaService: IOcenaService
{
    public IDriver _driver { get; set; }
    private readonly ICacheService _cache;

    public OcenaService(IDriver driver, ICacheService cache)
    {
        _driver = driver;
        _cache = cache;
    }

    public async Task<bool> addOcena(string userId,string majstorId)
    {
        IAsyncSession? session = null;
        try
        {
            session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write));
            var result=await session.RunAsync(@"MATCH (m:majstor{_id:$majstorId}) MATCH(k:korisnik{_id:$userId}) MERGE(k)-[r:OCENA]->(m) ON CREATE SET r.alreadyExisted=false ON MATCH set r.alreadyExisted=true RETURN k,r.alreadyExisted as existed",new
            {
                majstorId = majstorId,
                userId = userId
            });
            //Izvuci iz ove promenljive record ime i prezime korisnika i njegovu profilnu sliku i dodaj ih notifikaciju, izmeni ove variable
            var record = await result.SingleAsync();
            var korisnikNode = record["k"].As<INode>();
            string fullname = $"{korisnikNode.Properties["ime"]} {korisnikNode.Properties["prezime"]}";
            string profilePicture = korisnikNode.Properties.ContainsKey("profilePicture")
            ? korisnikNode.Properties["profilePicture"]?.ToString() ?? ""
            : "";
            var existed=record["existed"].As<bool>();
            if (existed == false)
            {
                var notifKey = $"notifications:{majstorId}";
                

                var notif = new NottificationDTO
                {
                    id = Guid.NewGuid().ToString(),
                    text =  $"{fullname} vas je dodao u kontakte",
                    time = DateTime.Now,          // ili DateTime.UtcNow ako svuda koristiš UTC
                    avatarUrl = string.IsNullOrWhiteSpace(profilePicture)
                    ? null
                    : "http://localhost:5104/images/" + profilePicture, // ili neki default / url korisnika ako ga imaš
                    korisnik = majstorId
                };

                var json = JsonSerializer.Serialize(notif);

            // Dodaj u Redis LIST (na kraj ili početak, kako ti odgovara)
                await _cache.ListRightPushAsync(notifKey, json);
                await _cache.PublishAsync("notification", json);
                }
            return existed==false;
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async Task<ListGetOcenaDTO> getOcena(string userId)
    {
        IAsyncSession? session = null;
        try
        {
            session= _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var result = await session.RunAsync(@"MATCH(k:korisnik{_id:$id})-[r:OCENA]->(m) RETURN k,r,m",new
            {
                id = userId
            });
            var records = await result.ToListAsync();
            ListGetOcenaDTO ocenaDto = new ListGetOcenaDTO();
            ocenaDto.ocene = new List<GetOcenaDTO>();
            foreach (var record in records)
            {
                var korisnik=record["k"].As<INode>();
                var ocena=record["r"].As<IRelationship>();
                var majstor=record["m"].As<INode>();
                var dto = new GetOcenaDTO
                {
                    _id = ocena.ElementId,
                    korisnik = korisnik.Properties["_id"].ToString(),
                    majstor = new GetMajstorDTO
                    {
                        _id = majstor.Properties["_id"].ToString(),
                        adresa = majstor.Properties["adresa"].ToString(),
                        broj = majstor.Properties["broj"].ToString(),
                        email = majstor.Properties["email"].ToString(),
                        ime = majstor.Properties["ime"].ToString(),
                        lokacija = majstor.Properties["lokacija"].ToString(),
                        podkategorija = new List<PodkategorijaDTO>(),
                        prezime = majstor.Properties["prezime"].ToString(),
                        profilePicture = "http://localhost:5104/images/" + majstor.Properties["profilePicture"],
                    }
                };
                if (ocena.Properties.ContainsKey("ocena"))
                {
                    dto.ocena=int.Parse(ocena.Properties["ocena"].ToString()!);
                }
                ocenaDto.ocene.Add(dto);
            }
            return ocenaDto;
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async Task<ListGetOcenaMajstorDTO> getOcenaByMajstorId(string majstorId)
    {
        IAsyncSession? session = null;
        try
        {
            session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var result = await session.RunAsync(@"MATCH(k)-[r:OCENA]->(m:majstor{_id:$majstorId}) RETURN k,r,m", new
            {
                majstorId = majstorId
            });
            var records = await result.ToListAsync();
            List<GetOcenaMajstorDTO> list = new List<GetOcenaMajstorDTO>();
            foreach (var record in records)
            {
                var korisnik = record["k"].As<INode>();
                var majstor=record["m"].As<INode>();
                var ocena=record["r"].As<IRelationship>();
                list.Add(new GetOcenaMajstorDTO
                {
                    _id = ocena.ElementId,
                    korisnik = new GetKlijentDTO
                    {
                      _id  = korisnik.Properties["_id"].ToString(),
                      ime = korisnik.Properties["ime"].ToString(),
                    },
                    majstor = majstor.Properties["_id"].ToString(),
                });
            }

            return new ListGetOcenaMajstorDTO
            {
                chats = list
            };
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async Task<bool> uploadOcena(string userId, string majstorId, int ocena)
    {    
        IAsyncSession? session = null;
        try
        {
            session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write));
            var result=await session.RunAsync(
                @"MATCH(k:korisnik{_id:$userId})-[r:OCENA]->(m:majstor{_id:$majstorId}) with r,k SET r.ocena=$ocena RETURN r,k",
                new
                {
                    majstorId = majstorId,
                    userId = userId,
                    ocena = ocena
                });
            //Izvuci iz ove promenljive record ime i prezime korisnika i njegovu profilnu sliku i dodaj ih notifikaciju, izmeni ove variable
            var record = await result.SingleAsync();
            var korisnikNode = record["k"].As<INode>();
            string fullname = $"{korisnikNode.Properties["ime"]} {korisnikNode.Properties["prezime"]}";
            string profilePicture = korisnikNode.Properties.ContainsKey("profilePicture")
            ? korisnikNode.Properties["profilePicture"]?.ToString() ?? ""
            : "";
            var notifKey = $"notifications:{majstorId}";
            var notif = new NottificationDTO
            {
                id = Guid.NewGuid().ToString(),
                text = $"{fullname} je ažurirao ocenu ({ocena}).",
                time = DateTime.Now,       // ili DateTime.UtcNow
                avatarUrl = string.IsNullOrWhiteSpace(profilePicture)
                    ? null
                    : "http://localhost:5104/images/" + profilePicture,
                korisnik = majstorId
            };

            var json = JsonSerializer.Serialize(notif);
            await _cache.ListRightPushAsync(notifKey, json);
            await _cache.PublishAsync("notification", json);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<double> averageOcena(string majstorId)
    {
        IAsyncSession? session = null;
        try
        {
            session= _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var result = await session.RunAsync(
                @"MATCH(k:korisnik)-[r:OCENA]->(m:majstor{_id:$id}) RETURN sum(r.ocena) as s,count(r.ocena) as c", new
                {
                    id = majstorId
                });
            var record = await result.SingleAsync();
            var sum = record["s"].As<double>();
            var count = record["c"].As<int>();
            if (count == 0)
            {
                return 0.0;
            }
            return sum/count;
        }
        catch (Exception e)
        {
            return 0.0;
        }
    }
}