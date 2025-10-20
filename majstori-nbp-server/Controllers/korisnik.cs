using majstori_nbp_server.DTOs.KlijentDTOs;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

public class korisnik : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost("/korisnik/signup")]
    public IActionResult signup([FromBody] CreateKlijentDTO createKlijentDTO)
    {
        Console.WriteLine(createKlijentDTO);
        return Ok();
    }
}