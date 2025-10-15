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

    [HttpGet("test")]
    public IActionResult Get() => Ok(new { Datum = DateTime.Now });

    [HttpGet(ApiEndpoints.V1.Majstori.Emails)]
    public IActionResult Emails()
    {
        return Ok(_majstorService.GetAllEmails());
    }

    [HttpGet(ApiEndpoints.V1.Majstori.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<GetMajstorDTO> majstori = await _majstorService.GetAllAsync();

        return Ok(majstori);
    }

    [HttpGet(ApiEndpoints.V1.Majstori.GetById)]
    public async Task<IActionResult> GetById(string id)
    {
        GetMajstorDTO? majstor = await _majstorService.GetByIdAsync(id);
        if (majstor != null)
        {
            return Ok(majstor);
        }

        return NotFound();
    }

    [HttpPost(ApiEndpoints.V1.Majstori.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMajstorDTO majstor)
    {
        GetMajstorDTO? noviMajstor = await _majstorService.CreateAsync(majstor);
        if (noviMajstor != null)
        {
            return CreatedAtAction(nameof(GetById), new { id = noviMajstor.Id.ToString() }, noviMajstor);
        }

        return BadRequest();
    }

    [HttpPut(ApiEndpoints.V1.Majstori.Update)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateMajstorDTO majstor)
    {
        GetMajstorDTO? azuriraniMajstor = await _majstorService.UpdateAsync(id, majstor);
        if (azuriraniMajstor != null)
        {
            return Ok(azuriraniMajstor);
        }

        return NotFound();
    }

    [HttpDelete(ApiEndpoints.V1.Majstori.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        bool isDeleted = await _majstorService.DeleteAsync(id);
        if (isDeleted)
        {
            return Ok();
        }

        return NotFound();
    }
}