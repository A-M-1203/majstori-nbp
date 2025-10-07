using majstori_nbp_server.Models;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

[ApiController]
[Route("[controller]")]
public class MajstoriController : ControllerBase
{
    private readonly ICacheService _cacheService;
    public MajstoriController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    // [HttpGet]
    // public async Task<IActionResult> GetAll()
    // {
    //     var data = await _cacheService.GetHashDataAsync("majstor:");

    //     List<Majstor> majstori = new(data.Length);
    //     foreach (var entry in data)
    //     {

    //     }
    // }
}