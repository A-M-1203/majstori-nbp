using majstori_nbp_server.DTOs.KlijentDTOs;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

[ApiController]
public class KlijentiController : ControllerBase
{
    private readonly IKlijentService _klijentService;

    public KlijentiController(IKlijentService klijentService)
    {
        _klijentService = klijentService;
    }

    [Authorize(Roles = "klijent")]
    [HttpGet("klijent")]
    public ActionResult KlijentOnly()
    {
        return Ok("Klijent");
    }

    [Authorize(Roles = "klijent,majstor")]
    [HttpGet("klijent-majstor")]
    public ActionResult KlijentAndMajstor()
    {
        return Ok("Klijent i majstor");
    }

    [HttpGet(ApiEndpoints.V1.Klijenti.Emails)]
    public async Task<IActionResult> Emails()
    {
        return Ok(await _klijentService.GetAllEmailsAsync());
    }

    [HttpGet(ApiEndpoints.V1.Klijenti.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(_klijentService.GetAllAsync());
    }

    [HttpGet(ApiEndpoints.V1.Klijenti.GetById)]
    public async Task<IActionResult> GetById(string id)
    {
        GetKlijentDTO? majstor = await _klijentService.GetByIdAsync(id);
        if (majstor is not null)
        {
            return Ok(majstor);
        }

        return Problem
        (
            type: "Not Found",
            title: "Klijent ne postoji",
            detail: "Klijent sa navedenim Id-jem ne postoji",
            statusCode: StatusCodes.Status404NotFound
        );
    }

    [HttpPost(ApiEndpoints.V1.Klijenti.Create)]
    public async Task<IActionResult> Create([FromBody] CreateKlijentDTO klijent)
    {
        GetKlijentDTO? noviKlijent = await _klijentService.CreateAsync(klijent);
        if (noviKlijent is not null)
        {
            return CreatedAtAction(nameof(GetById), new { id = noviKlijent.Id.ToString() }, noviKlijent);
        }

        return Problem
        (
            type: "Bad Request",
            title: "Klijent nije kreiran",
            detail: "Vec postoji nalog sa navedenom email adresom",
            statusCode: StatusCodes.Status400BadRequest
        );
    }

    [HttpPut(ApiEndpoints.V1.Klijenti.Update)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateKlijentDTO klijent)
    {
        GetKlijentDTO? azuriraniKlijent = await _klijentService.UpdateAsync(id, klijent);
        if (azuriraniKlijent is not null)
        {
            return Ok(azuriraniKlijent);
        }

        return Problem
        (
            type: "Not Found",
            title: "Klijent ne postoji",
            detail: "Klijent sa navedenim Id-jem ne postoji",
            statusCode: StatusCodes.Status404NotFound
        );
    }

    [HttpDelete(ApiEndpoints.V1.Klijenti.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        bool isDeleted = await _klijentService.DeleteAsync(id);
        if (isDeleted)
        {
            return Ok();
        }

        return Problem
        (
            type: "Not Found",
            title: "Klijent ne postoji",
            detail: "Klijent sa navedenim Id-jem ne postoji",
            statusCode: StatusCodes.Status404NotFound
        );
    }
}