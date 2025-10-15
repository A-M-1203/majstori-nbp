using majstori_nbp_server.DTOs.MajstorDTOs;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

[ApiController]
public class MajstoriController : ControllerBase
{
    private readonly IMajstorService _majstorService;

    public MajstoriController(IMajstorService majstorService)
    {
        _majstorService = majstorService;
    }

    [HttpGet(ApiEndpoints.V1.Majstori.Emails)]
    public IActionResult Emails()
    {
        return Ok(_majstorService.GetAllEmails());
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

    [HttpPost(ApiEndpoints.V1.Majstori.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMajstorDTO majstor)
    {
        GetMajstorDTO? noviMajstor = await _majstorService.CreateAsync(majstor);
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
        );
    }

    [HttpPut(ApiEndpoints.V1.Majstori.Update)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateMajstorDTO majstor)
    {
        GetMajstorDTO? azuriraniMajstor = await _majstorService.UpdateAsync(id, majstor);
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