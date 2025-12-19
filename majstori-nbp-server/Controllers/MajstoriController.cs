using majstori_nbp_server.DTOs;
using majstori_nbp_server.DTOs.MajstorDTOs;
using majstori_nbp_server.Middleware;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace majstori_nbp_server.Controllers;

[ApiController]
[Route("[controller]")]
public class majstorController : ControllerBase
{
    private readonly IMajstorService _majstorService;
    private readonly IOcenaService _ocenaService;

    public majstorController(IMajstorService majstorService,IOcenaService ocenaService)
    {
        _majstorService = majstorService;
        _ocenaService = ocenaService;
    }

    [HttpGet(ApiEndpoints.V1.Majstori.Emails)]
    public IActionResult Emails()
    {
        return Ok(_majstorService.GetAllEmails());
    }

    [HttpPost("signin")]
    public async Task<IActionResult> signin([FromBody]LoginDTO loginDTO)
    {
        string token = await _majstorService.SignIn(loginDTO.email, loginDTO.password);
        if (token == "")
        {
            string message = "Log in neuspesan";
            var data = new { message = message };
            return NotFound(new JsonResult(data));
        }
        else
        {
            string message = "Log in uspesan";
            string type = "majstor";
            var data = new { message = message, type = type, token = token };
            return Ok(new JsonResult(data).Value);
        }
    }

    [HttpGet("profile")]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<IActionResult> GetProfile()
    {
        bool has=Authorization.Authorization.IsMajstor(HttpContext.Items["role"] as string);
        if (!has)
        {
            return Unauthorized();
        }
        string userId=HttpContext.Items["userId"] as string;
        var majstor = await _majstorService.GetByIdAsync(userId);
        return Ok(majstor);
    }
    
    [HttpGet(ApiEndpoints.V1.Majstori.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(_majstorService.GetAllAsync());
    }

    [HttpGet(ApiEndpoints.V1.Majstori.GetById)]
    public async Task<IActionResult> GetById(string id)
    {
        GetMajstorDTO? majstor = await _majstorService.GetByIdAsync(id);
        if (majstor is not null)
        {
            return Ok(majstor);
        }

        return Problem
        (
            type: "Not Found",
            title: "Majstor ne postoji",
            detail: "Majstor sa navedenim Id-jem ne postoji",
            statusCode: StatusCodes.Status404NotFound
        );
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Create([FromBody] CreateMajstorDTO majstor)
    {
       /* GetMajstorDTO? noviMajstor = await _majstorService.CreateAsync(majstor);
        if (noviMajstor is not null)
        {
            return CreatedAtAction(nameof(GetById), new { id = noviMajstor.Id.ToString() }, noviMajstor);
        }

        return Problem
        (
            type: "Bad Request",
            title: "Majstor nije kreiran",
            detail: "Vec postoji nalog sa navedenom email adresom",
            statusCode: StatusCodes.Status400BadRequest
        );*/
       bool success = await _majstorService.CreateAsync(majstor);
       if (success)
       {
           return Ok("majstori created");
       }
       else
       {
           return BadRequest("majstor not created");
       }
    }

    [HttpPut]
    [Route("")]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<IActionResult> Update( [FromForm] UpdateMajstorDTO majstor)
    {
        string userId=HttpContext.Items["userId"] as string;
        GetMajstorDTO? azuriraniMajstor = await _majstorService.UpdateAsync(userId,majstor);
        if (azuriraniMajstor is not null)
        {
            return Ok(azuriraniMajstor);
        }

        return Problem
        (
            type: "Not Found",
            title: "Majstor ne postoji",
            detail: "Majstor sa navedenim Id-jem ne postoji",
            statusCode: StatusCodes.Status404NotFound
        );
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetMajstor(string id)
    {
        GetMajstorDTO? majstor = await _majstorService.GetByIdAsync(id);
        double prosek =await _ocenaService.averageOcena(id);
        if (majstor is not null)
        {
            return Ok(new FullMajstorDTO
            {
                majstor = majstor,
                prosek = prosek
            });
        }
        else
        {
            return NotFound("Majstor ne postoji");
        }
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetMajstors([FromQuery]string? kategorija,[FromQuery]string? podkategorija)
    {
        var result=await _majstorService.getMajstorsByKategorijaId(kategorija, podkategorija);
        return Ok(new MajstorFilterDTO
        {
            message = "Majstors fetched",
            majstors = result
        });
    }

    [HttpDelete(ApiEndpoints.V1.Majstori.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        bool isDeleted = await _majstorService.DeleteAsync(id);
        if (isDeleted)
        {
            return Ok();
        }

        return Problem
        (
            type: "Not Found",
            title: "Majstor ne postoji",
            detail: "Majstor sa navedenim Id-jem ne postoji",
            statusCode: StatusCodes.Status404NotFound
        );
    }
}