using majstori_nbp_server.DTOs.KategorijeDTOs;
using majstori_nbp_server.Services;
using Neo4j.Driver;

namespace majstori_nbp_server.Implementations;

public class KategorijaService:IKategorijaService
{
    private readonly IDriver _driver;

    public KategorijaService(IDriver driver)
    {
        _driver = driver;
    }
    public async Task<KategorijeDTO> GetKategorijas()
    {
        var kategorija = new List<KategorijaDTO>();
        var kategorijeRes=new KategorijeDTO();
        IAsyncSession? session = null;
        try
        {
            session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));

            //izmena [x IN podkategorija WHERE x IS NOT NULL] je da ne dobiješ [null] kad neka kategorija nema podkategorije.
            var kategorije = await session.RunAsync(@"
                MATCH (k:Kategorija)
                OPTIONAL MATCH (k)-[:PODKATEGORIJA|IMA]->(p:Podkategorija)
                WITH k, collect(DISTINCT p) AS podkategorija
                RETURN k, [x IN podkategorija WHERE x IS NOT NULL] AS podkategorija
            ");
            var records = await kategorije.ToListAsync();
            foreach (var record in records)
            {
                var node = record["k"].As<INode>();
                var podList=record["podkategorija"].As<IList<INode>>();
                var kategorijaDTO = new KategorijaDTO
                {
                    _id = node.Properties.ContainsKey("_id") ? node.Properties["_id"]?.As<string>() : "",
                    naziv = node.Properties.ContainsKey("naziv") ? node.Properties["naziv"]?.As<string>() : "",
                };
                if (podList != null)
                {
                    foreach (var podkategorija in podList)
                    {
                        var pod=new PodkategorijaDTO()
                        {
                            _id=podkategorija.Properties.ContainsKey("_id") ? podkategorija.Properties["_id"]?.As<string>() : "",
                            naziv = podkategorija.Properties.ContainsKey("naziv")?podkategorija.Properties["naziv"]?.As<string>() : "",
                            kategorija = kategorijaDTO._id
                        };
                        kategorijaDTO.podkategorije.Add(pod);
                    }
                }
                kategorija.Add(kategorijaDTO);
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            if (session != null)
                await session.CloseAsync();
        }

        kategorijeRes.kategorije = kategorija;
        return kategorijeRes;
    }
}