using majstori_nbp_server.DTOs.KategorijeDTOs;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class kategorijeController : ControllerBase
{
    private readonly IKategorijaService _service;

    public kategorijeController(IKategorijaService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<KategorijeDTO> GetKategorija()
    {
        return await _service.GetKategorijas();
    }
}