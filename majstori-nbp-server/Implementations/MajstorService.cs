using majstori_nbp_server.DTOs.KategorijeDTOs;
using majstori_nbp_server.DTOs.MajstorDTOs;
using majstori_nbp_server.Helper;
using majstori_nbp_server.Mappings;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using Neo4j.Driver.Mapping;
using System.Text.Json;


namespace majstori_nbp_server.Implementations;

public class MajstorService : IMajstorService
{
    private readonly ICacheService _cacheService;
    private readonly IEmailService _emailService;
    private readonly IDriver _driver;
    private readonly JwtSecurityTokenHandlerWrapper _wrapper;
    private readonly IKategorijaService _kategorijaService;
    private static string SessionKey(string token) => $"session:{token}";

    public MajstorService(ICacheService cacheService, IEmailService emailService,IKategorijaService kategorijaService, IDriver driver,JwtSecurityTokenHandlerWrapper wrapper)
    {
        _cacheService = cacheService;
        _emailService = emailService;
        _driver = driver;
        _wrapper = wrapper;
        _kategorijaService = kategorijaService;
    }

    public IEnumerable<string> GetAllEmails()
    {
        return _emailService.GetAllEmails();
    }

    public IAsyncEnumerable<GetMajstorDTO> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    /*public async IAsyncEnumerable<GetMajstorDTO> GetAllAsync()
    {
        await foreach (var (key, entries) in _cacheService.GetAllHashDataAsync("majstor:*"))
        {
            GetMajstorDTO majstor = new();
            string id = key.Substring("majstor:".Length);
            majstor.Id = Guid.Parse(id);

            majstor.Ime = entries.FirstOrDefault(x => x.Name == "ime").Value!;
            majstor.Prezime = entries.FirstOrDefault(x => x.Name == "prezime").Value!;
            majstor.BrojTelefona = entries.FirstOrDefault(x => x.Name == "brojtelefona").Value!;
            majstor.Email = entries.FirstOrDefault(x => x.Name == "email").Value!;

            yield return majstor;

        }
    }*/
    public async Task<bool> ExistsEmail(string email)
    {
        IAsyncSession? session = null;
        try
        {
            session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var result = await session.RunAsync(@"MATCH (u:majstor{email:$email}) RETURN u", new { email });
            var user = await result.ToListAsync();
            return user.Count > 0;
        }
        finally
        {
            session?.Dispose();    
        }
    }

    public async Task<GetMajstorDTO?> GetByIdAsync(string id)
    {
        IAsyncSession? session = null;
        try
        {
            session= _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var result = await session.RunAsync(@"MATCH (u:majstor{_id:$id}) MATCH(u)-[:WORKS_IN]->(p:Podkategorija) RETURN u,collect(p) as podkategorija", new
            {
                id = id
            });
            var user = await result.SingleAsync();
            var node = user["u"].As<INode>();
            var podkategorija=user["podkategorija"].As<IList<INode>>();
            List<PodkategorijaDTO>list = new List<PodkategorijaDTO>();
            foreach (var p in podkategorija)
            {
                list.Add(new PodkategorijaDTO
                {
                    _id=p.Properties["_id"].As<string>(),
                    naziv = p.Properties["naziv"].As<string>(),
                });
            }
            
            return new GetMajstorDTO
            {
                _id = node.Properties["_id"].ToString(),
                ime = node.Properties["ime"].ToString(),
                prezime = node.Properties["prezime"].ToString(),
                adresa = node.Properties["adresa"].ToString(),
                broj = node.Properties["broj"].ToString(),
                email = node.Properties["email"].ToString(),
                lokacija = node.Properties["lokacija"].ToString(),
                profilePicture = "http://localhost:5104/images/" + node.Properties["profilePicture"],
                podkategorija = list,
            };
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async Task<bool> CreateAsync(CreateMajstorDTO majstor)
    {
       /* bool isCreated = await _emailService.CreateEmailAsync(majstor.email);
        if (isCreated is false)
        {
            return null;
        }

        string id = Guid.NewGuid().ToString();
        string key = "majstor:" + id;
        var entries = await _cacheService.CreateHashDataAsync(key, majstor);
        GetMajstorDTO? noviMajstor = null;
        if (entries is not null)
        {
            noviMajstor = entries.MapToGetMajstorDTO(id);
        }

        return noviMajstor;*/
       var has = await ExistsEmail(majstor.email);
       if (has)
       {
           return false;
       }
       IAsyncSession? session = null;
       var hashPassword = BCrypt.Net.BCrypt.HashPassword(majstor.password);
       string userId = Ulid.NewUlid().ToString();
       try
       {
           session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write));
           var result = session.RunAsync(@"CREATE (m:majstor{
                _id:$userId,email:$email,password:$password,broj:$broj,ime:$ime,prezime:$prezime,adresa:$adresa,lokacija:$lokacija,profilePicture:$profilePicture}) WITH m MATCH(k:Podkategorija) WHERE k._id IN $podkategorija MERGE (m)-[:WORKS_IN]->(k) RETURN m ",
               new
               {
                   userId = userId,
                   email = majstor.email,
                   password = hashPassword,
                   broj = majstor.broj,
                   ime = majstor.ime,
                   prezime = majstor.prezime,
                   adresa = majstor.adresa,
                   lokacija = majstor.lokacija,
                   podkategorija = majstor.podkategorija,
                   profilePicture="default-profile.jpg"
               });
           return true;
       }
       finally
       {
           session?.Dispose();
       }
    }
    
    public async Task<GetMajstorDTO?> UpdateAsync( string id,[FromForm] UpdateMajstorDTO majstor)
    {

        string filename = null;

        if (majstor.image != null)
        {
            filename = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString() + majstor.image.FileName;
            using (var stream = new FileStream("images/" + filename, FileMode.Create))
            {
                await majstor.image.CopyToAsync(stream);
            }
        }

        IAsyncSession? session = null;
        IAsyncSession? session2 = null;
        try
        {
            session2 = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write));
            session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write));
            var result2=await session2.RunAsync(
                @"Match(m:majstor{_id:$id}) MATCH(m)-[r:WORKS_IN]->(p:Podkategorija) DELETE r WITH m MATCH(m) MATCH(p1:Podkategorija) where p1._id in $ids MERGE(m)-[:WORKS_IN]->(p1) RETURN p1",new
                {
                    id = id,
                    ids=majstor.podkategorija
                });
            //var pods = await result2.ToListAsync();

            Console.WriteLine("Current podkategorije:");
            /*foreach (var recordPod in pods)
            {
                Console.WriteLine("- " + recordPod["_id"].As<string>());
            }*/
            IResultCursor result = null;
            if (filename != null)
            {
                result = await session.RunAsync(
                    @"MATCH(m:majstor{_id:$id}) WITH m SET m.ime=$ime SET m.prezime=$prezime SET m.broj=$broj  SET m.adresa=$adresa SET m.lokacija=$lokacija SET m.profilePicture=$filename RETURN m",
                    new
                    {
                        id = id,
                        ime = majstor.ime,
                        prezime = majstor.prezime,
                        adresa = majstor.adresa,
                        lokacija = majstor.lokacija,
                        broj = majstor.broj,
                        filename = filename
                    });
            }
            else
            {
                result = await session.RunAsync(
                    @"MATCH(m:majstor{_id:$id}) WITH m SET m.ime=$ime SET m.prezime=$prezime SET m.broj=$broj  SET m.adresa=$adresa SET m.lokacija=$lokacija  RETURN m",
                    new
                    {
                        id = id,
                        ime = majstor.ime,
                        prezime = majstor.prezime,
                        adresa = majstor.adresa,
                        lokacija = majstor.lokacija,
                        broj = majstor.broj,
                        filename = filename
                    });
            }
            var record = await result.SingleAsync();
            var node = record["m"].As<INode>();
            return new GetMajstorDTO
            {
                _id = node.Properties["_id"].ToString(),
                ime = node.Properties["ime"].ToString(),
                prezime = node.Properties["prezime"].ToString(),
                adresa = node.Properties["adresa"].ToString(),
                broj = node.Properties["broj"].ToString(),
                email = node.Properties["email"].ToString(),
                lokacija = node.Properties["lokacija"].ToString(),
                profilePicture = "http://localhost:5104/images/" + node.Properties["profilePicture"],
                podkategorija = new List<PodkategorijaDTO>()
            };
                
        }
        finally
        {
            session?.Dispose();
        }
    }

    public Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    

    /*public async Task<GetMajstorDTO?> UpdateAsync(string id, UpdateMajstorDTO majstor)
    {
        GetMajstorDTO? p = await GetByIdAsync(id);
        if (p != null && majstor.Email != null)
        {
            await _emailService.UpdateEmailAsync(majstor.Email, p.Email);
        }

        string key = "majstor:" + id;
        var entries = await _cacheService.UpdateHashDataAsync(key, majstor);
        GetMajstorDTO? azuriraniMajstor = null;
        if (entries is not null)
        {
            azuriraniMajstor = entries.MapToGetMajstorDTO(id);
        }

        return azuriraniMajstor;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        GetMajstorDTO? p = await GetByIdAsync(id);
        if (p is not null)
        {
            await _emailService.DeleteEmailAsync(p.Email);
        }
        string key = "majstor:" + id;
        bool isDeleted = await _cacheService.DeleteHashDataAsync(key);

        return isDeleted;
    }*/

    public async Task<string> SignIn(string email, string password)
    {
        IAsyncSession? session = null;
        try
        {
            session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var result = await session.RunAsync(@"MATCH (u:majstor{email:$email}) RETURN u", new { email });
            var record = await result.ToListAsync();

            if (record is null || record.Count != 1) return "";

            var node = record[0]["u"].As<INode>();
            bool verify = BCrypt.Net.BCrypt.Verify(password, node.Properties["password"]?.As<string>() ?? "");
            if (!verify) return "";

            var userId = node.Properties["_id"]?.As<string>() ?? "";
            var token = TokenGen.NewToken();

            var sessionObj = new { userId = userId, role = "majstor" };
            var sessionJson = JsonSerializer.Serialize(sessionObj);

            await _cacheService.SetStringAsync(SessionKey(token), sessionJson, TimeSpan.FromMinutes(30));

            return token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return "";
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async Task<List<GetMajstorDTO>> getMajstorsByKategorijaId(string? kategorija, string? podkategorija)
    {
        IAsyncSession? session = null;
        try
        {
            session= _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            IResultCursor? result = null;
            if (podkategorija is not null)
            {
                result = await session.RunAsync(@"MATCH(m)-[:WORKS_IN]->(p:Podkategorija{_id:$id}) RETURN m",new
                {
                    id = podkategorija,
                });
            }
            else
            {
                result = await session.RunAsync(@"MATCH(p)-[:BELONGS]->(k:Kategorija{_id:$id}) MATCH(m)-[:WORKS_IN]->(p) RETURN DISTINCT    m",new
                {
                    id = kategorija
                });
            }
            var list=new List<GetMajstorDTO>();
            var records = await result.ToListAsync();
            foreach (var record in records)
            {
                var node = record["m"].As<INode>();
                list.Add(new GetMajstorDTO
                {
                    _id = node.Properties["_id"].ToString(),
                    ime = node.Properties["ime"].ToString(),
                    prezime = node.Properties["prezime"].ToString(),
                    adresa = node.Properties["adresa"].ToString(),
                    broj =  node.Properties["broj"].ToString(),
                    email = node.Properties["email"].ToString(),
                    lokacija = node.Properties["lokacija"].ToString(),
                    podkategorija = new List<PodkategorijaDTO>(),
                    profilePicture = "http://localhost:5104/images/" + node.Properties["profilePicture"]
                });
            }

            return list;
        }finally
        {
            session?.Dispose();
        }
    }
}