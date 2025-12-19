using majstori_nbp_server.DTOs.KategorijeDTOs;
using majstori_nbp_server.DTOs.KlijentDTOs;
using majstori_nbp_server.DTOs.MajstorDTOs;
using majstori_nbp_server.DTOs.Ocena;
using majstori_nbp_server.Services;
using Neo4j.Driver;

namespace majstori_nbp_server.Implementations;

public class OcenaService: IOcenaService
{
    public IDriver _driver { get; set; }

    public OcenaService(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<bool> addOcena(string userId,string majstorId)
    {
        IAsyncSession? session = null;
        try
        {
            session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write));
            var result=await session.RunAsync(@"MATCH (m:majstor{_id:$majstorId}) MATCH(k:korisnik{_id:$userId}) MERGE(k)-[r:OCENA]->(m) ON CREATE SET r.alreadyExisted=false ON MATCH set r.alreadyExisted=true RETURN r.alreadyExisted as existed",new
            {
                majstorId = majstorId,
                userId = userId
            });
            var record = await result.SingleAsync();
            var existed=record["existed"].As<bool>();
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
                @"MATCH(k:korisnik{_id:$userId})-[r:OCENA]->(m:majstor{_id:$majstorId}) with r SET r.ocena=$ocena RETURN r",
                new
                {
                    majstorId = majstorId,
                    userId = userId,
                    ocena = ocena
                });
            await result.SingleAsync();
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