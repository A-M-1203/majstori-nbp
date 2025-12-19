using majstori_nbp_server.DTOs.Ocena;
using majstori_nbp_server.Middleware;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

[ApiController]
[Route("[controller]")]
public class ocenaController:ControllerBase
{
    private readonly IOcenaService _ocenaService;

    public ocenaController(IOcenaService ocenaService)
    {
        _ocenaService = ocenaService;
    }

    [HttpPost]
    [Route("")]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<ActionResult> add([FromBody] AddOcenaDTO majstorId)
    {
        //Dodaj notifikaciju koja obavestava majstora da je dodat u kontakte
        string userId=HttpContext.Items["userId"] as string;
        if (userId == null)
        {
            return Unauthorized();
        }
        string role=HttpContext.Items["role"] as string;
        if (role == null || !Authorization.Authorization.IsKorisnik(role))
        {
            return Unauthorized();
        }
        bool addOp=await _ocenaService.addOcena(userId, majstorId.majstorId);
        if (addOp)
        {
            return Ok("Ocena added");
        }
        else
        {
            return BadRequest("Ocena not added or already exists");
        }
    }
    
    [HttpGet]
    [Route("")]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<ActionResult> get()
    {
        string userId=HttpContext.Items["userId"] as string;
        if (userId == null)
        {
            return Unauthorized();
        }
        string role=HttpContext.Items["role"] as string;
        if (role == null || !Authorization.Authorization.IsKorisnik(role))
        {
            return Unauthorized();
        }

        try
        {
            var ocena = await _ocenaService.getOcena(userId);
            return Ok(ocena);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("chats")]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<IActionResult> chats()
    {
        string userId=HttpContext.Items["userId"] as string;
        if (userId == null)
        {
            return Unauthorized();
        }
        string role=HttpContext.Items["role"] as string;
        if (role == null || !Authorization.Authorization.IsMajstor(role))
        {
            return Unauthorized();
        }

        try
        {
            var ocena = await _ocenaService.getOcenaByMajstorId(userId);
            return Ok(ocena);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpPut]
    [Route("")]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<ActionResult> update([FromBody] UpdateOcenaDTO updateOcena)
    {
        //Dodaj notifijakaciju koja govori majstoru sa kojom ocenom je ocenjen
        string userId=HttpContext.Items["userId"] as string;
        if (userId == null)
        {
            return Unauthorized();
        }
        string role=HttpContext.Items["role"] as string;
        if (role == null || !Authorization.Authorization.IsKorisnik(role))
        {
            return Unauthorized();
        }
        bool updateSuc=await _ocenaService.uploadOcena(userId, updateOcena.majstorId,updateOcena.ocena);
        if (updateSuc)
        {
            return Ok("Ocena updated");
        }
        else
        {
            return BadRequest("Ocena not updated");
        }
    }
}