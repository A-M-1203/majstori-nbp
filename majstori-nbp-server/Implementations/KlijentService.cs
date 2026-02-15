using majstori_nbp_server.DTOs.KlijentDTOs;
using majstori_nbp_server.Helper;
using majstori_nbp_server.Mappings;
using majstori_nbp_server.Services;
using Neo4j.Driver;
using System.Security.Cryptography;
using System.Text.Json;


namespace majstori_nbp_server.Implementations;

public class KlijentService : IKlijentService
{
    private readonly ICacheService _cacheService;
    private readonly IEmailService _emailService;
    private readonly IDriver _driver;
    private readonly JwtSecurityTokenHandlerWrapper _wrapper;




    public KlijentService(ICacheService cacheService, IEmailService emailService, IDriver driver, JwtSecurityTokenHandlerWrapper wrapper)
    {
        _wrapper = wrapper;
        _cacheService = cacheService;
        _emailService = emailService;
        _driver = driver;
    }

    public IEnumerable<string> GetAllEmails()
    {
        return _emailService.GetAllEmails();
    }

    public IAsyncEnumerable<GetKlijentDTO> GetAllAsync()
    {
       /* await foreach (var (key, entries) in _cacheService.GetAllHashDataAsync("klijent:*"))
        {
            GetKlijentDTO klijent = new();
            string id = key.Substring("klijent:".Length);
            klijent.Id = Guid.Parse(id);

            klijent.Ime = entries.FirstOrDefault(x => x.Name == "ime").Value!;
            klijent.Prezime = entries.FirstOrDefault(x => x.Name == "prezime").Value!;
            klijent.BrojTelefona = entries.FirstOrDefault(x => x.Name == "brojtelefona").Value!;
            klijent.Email = entries.FirstOrDefault(x => x.Name == "email").Value!;

            yield return klijent;

        }*/
       throw new NotImplementedException();
    }

    public async Task<GetKlijentDTO?> GetByIdAsync(string id)
    {
       /* string key = "klijent:" + id;
        var entries = await _cacheService.GetHashDataAsync(key);
        GetKlijentDTO? klijent = null;
        if (entries is not null)
        {
            klijent = entries.MapToGetKlijentDTO(id);
        }

        return klijent;*/
       IAsyncSession? session = null;
       try
       {
            session=_driver.AsyncSession(o=>o.WithDefaultAccessMode(AccessMode.Read));
            var result = await session.RunAsync(@"MATCH (k:korisnik{_id:$id}) RETURN k", new
            {
                id = id
            });
            var record = await result.SingleAsync();
            var node = record["k"].As<INode>();
            return new GetKlijentDTO
            {
                _id = node.Properties["_id"].ToString(),
                adresa = node.Properties["adresa"].ToString(),
                broj = node.Properties["broj"].ToString(),
                email = node.Properties["email"].ToString(),
                ime = node.Properties["ime"].ToString(),
                lokacija = node.Properties["lokacija"].ToString(),
                prezime = node.Properties["prezime"].ToString(),
                profilePicture = "http://localhost:5104/images/" + node.Properties["profilePicture"].ToString(),
            };
       }
       finally
       {
           
       }
    }

    public async Task<bool> CreateAsync(CreateKlijentDTO klijent)
    { 
        /*bool isCreated = await _emailService.CreateEmailAsync(klijent.Email);
        if (isCreated is false)
        {
            return null;
        }
        */
        /*string id = Guid.NewGuid().ToString();
        string key = "klijent:" + id;
        var entries = await _cacheService.CreateHashDataAsync(key, klijent);
        GetKlijentDTO? noviKlijent = null;
        if (entries is not null)
        {
            noviKlijent = entries.MapToGetKlijentDTO(id);
        }

        return noviKlijent;*/
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(klijent.password);
        string userId = Ulid.NewUlid().ToString();
        IAsyncSession? session = null;
        try
        {
            session =  _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write));
            await session.RunAsync(@"CREATE (u:korisnik{
                _id:$userId,email:$email,password:$password,broj:$broj,ime:$ime,prezime:$prezime,adresa:$adresa,lokacija:$lokacija,profilePicture:$profilePicture})",new 
            {
                userId=userId,
                email=klijent.email,
                password=hashPassword,
                broj=klijent.broj,
                ime=klijent.ime,
                prezime=klijent.prezime,
                adresa=klijent.adresa,
                lokacija=klijent.lokacija,
                profilePicture="default-profile.jpg"
            });
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    public async Task<bool> ExistsEmail(string email)
    {
        IAsyncSession? session = null;
        try
        {
            session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var result = await session.RunAsync(@"MATCH (u:korisnik{email:$email}) RETURN u", new { email });
            var user = await result.ToListAsync();
            session?.Dispose();   
            return user.Count > 0;
        }
        finally
        {
            session?.Dispose();    
        }
    }

    public async Task<GetKlijentDTO?> UpdateAsync(string id, UpdateKlijentDTO klijent)
    {
        /*GetKlijentDTO? p = await GetByIdAsync(id);
        if (p != null && klijent.Email != null)
        {
            await _emailService.UpdateEmailAsync(klijent.Email, p.Email);
        }

        string key = "klijent:" + id;
        var entries = await _cacheService.UpdateHashDataAsync(key, klijent);
        GetKlijentDTO? azuriraniKlijent = null;
        if (entries is not null)
        {
            azuriraniKlijent = entries.MapToGetKlijentDTO(id);
        }

        return azuriraniKlijent;*/
        string filename = null;

        if (klijent.image != null)
        {
            filename = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString() + klijent.image.FileName;
            using (var stream = new FileStream("images/" + filename, FileMode.Create))
            {
                await klijent.image.CopyToAsync(stream);
            }
        }
        IAsyncSession? session = null;
        try
        {
            session=_driver.AsyncSession(o=>o.WithDefaultAccessMode(AccessMode.Write));
            IResultCursor? cursor = null;
            if (filename != null)
            {
                cursor = await session.RunAsync(
                    @"MATCH (k:korisnik{_id:$id}) WITH k SET k.ime=$ime SET k.prezime=$prezime SET k.broj=$broj  SET k.adresa=$adresa SET k.lokacija=$lokacija SET k.profilePicture=$filename RETURN k",new
                    {
                        id=id,
                        ime=klijent.ime,
                        prezime=klijent.prezime,
                        broj=klijent.broj,
                        adresa=klijent.adresa,
                        lokacija=klijent.lokacija,
                        filename=filename,
                    });
            }
            else
            {
                cursor = await session.RunAsync(
                    @"MATCH (k:korisnik{_id:$id}) WITH k SET k.ime=$ime SET k.prezime=$prezime SET k.broj=$broj  SET k.adresa=$adresa SET k.lokacija=$lokacija  RETURN k",new
                    {
                        id=id,
                        ime=klijent.ime,
                        prezime=klijent.prezime,
                        broj=klijent.broj,
                        adresa=klijent.adresa,
                        lokacija=klijent.lokacija
                    });
            }
            var record = await cursor.SingleAsync();
            var node=record["k"].As<INode>();
            return new GetKlijentDTO
            {
                _id = node.Properties["_id"].ToString(),
                adresa = node.Properties["adresa"].ToString(),
                broj = node.Properties["broj"].ToString(),
                email = node.Properties["email"].ToString(),
                ime = node.Properties["ime"].ToString(),
                lokacija = node.Properties["lokacija"].ToString(),
                prezime = node.Properties["prezime"].ToString(),
                profilePicture = "http://localhost:5104/images/" + node.Properties["profilePicture"],
            };
        }
        finally
        {
            session?.Dispose();
        }
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        /*GetKlijentDTO? p = await GetByIdAsync(id);
        if (p is not null)
        {
            await _emailService.DeleteEmailAsync(p.Email);
        }
        string key = "klijent:" + id;
        bool isDeleted = await _cacheService.DeleteHashDataAsync(key);

        return isDeleted;*/
        throw new NotImplementedException();
    }


//HELPER METODE(redis)
    // private static string GenerateToken(int bytes = 32)
    // {
    //     // 32 bytes = 256-bit random, dovoljno jako
    //     var buffer = new byte[bytes];
    //     RandomNumberGenerator.Fill(buffer);
    //     return Convert.ToBase64String(buffer)
    //         .Replace("+", "-")
    //         .Replace("/", "_")
    //         .TrimEnd('=');
    // }

    private static string SessionKey(string token) => $"session:{token}";


    public async Task<string> SignIn(string email, string password)
    {
        IAsyncSession? session = null;
        try
        {
            session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var result = await session.RunAsync(@"MATCH (u:korisnik{email:$email}) RETURN u", new
            {
                email = email,
                password = password
            });
            var record = await result.ToListAsync();
            if (record is not null && record.Count==1)
            {
                var node = record[0]["u"].As<INode>();
                bool verify = BCrypt.Net.BCrypt.Verify(password, node.Properties["password"]?.As<string>() ?? "");

            //IZMENA IF-a    
            if (verify)
            {
                var userId = node.Properties["_id"]?.As<string>() ?? "";
                var token = TokenGen.NewToken(); // <-- OVDE(ZA RANDOM STRINg to jest token)

                var sessionObj = new { userId = userId, role = "korisnik" };
                var sessionJson = JsonSerializer.Serialize(sessionObj);

                // čuvamo 30 minuta (posle automatski nestaje)
                await _cacheService.SetStringAsync(SessionKey(token), sessionJson, TimeSpan.FromMinutes(30));

                return token;
            }

                return "";
            }

            session?.Dispose();
            return "";

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return "";
        }
    }
}