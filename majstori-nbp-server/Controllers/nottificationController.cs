using majstori_nbp_server.DTOs.NottificationDTOs;
using majstori_nbp_server.Middleware;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;

namespace majstori_nbp_server.Controllers;

[ApiController]
[Route("[controller]")]
public class nottificationController:ControllerBase
{
  private readonly ICacheService _cache;

    public nottificationController(ICacheService cache)
    {
        _cache = cache;
    }

    [HttpGet]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<ActionResult> Get()
    {
        string id = HttpContext.Items["userId"] as string;

        // Redis key: per-user lista notifikacija.
        // Svaki element u listi je JSON (NottificationDTO).
        // Primer: notifications:<userId>
        var key = string.IsNullOrWhiteSpace(id) ? "notifications" : $"notifications:{id}";

        var raw = await _cache.ListRangeAsync(key, 0, -1);
        if (raw.Length == 0)
        {
            return Ok(new List<NottificationDTO>());
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        jsonOptions.Converters.Add(new DateTimeConverter());

        var list = new List<NottificationDTO>();
        foreach (var item in raw)
        {
            if (string.IsNullOrWhiteSpace(item))
                continue;

            try
            {
                var dto = JsonSerializer.Deserialize<NottificationDTO>(item, jsonOptions);
                if (dto != null)
                    list.Add(dto);
            }
            catch
            {
                // Ako se u Redis-u nađe nevalidan JSON, preskoči element.
            }
        }

        // Najnovije prvo
        list = list.OrderByDescending(n => n.time).ToList();

        return Ok(list);
    }  

    [HttpDelete]
[ServiceFilter(typeof(JwtAuthorizeFilter))]
public async Task<IActionResult> Delete([FromQuery] string? notificationId)
{
    string id = HttpContext.Items["userId"] as string;
    var key = string.IsNullOrWhiteSpace(id) ? "notifications" : $"notifications:{id}";

    // Ako nema notificationId -> obriši sve notifikacije (ceo key)
    if (string.IsNullOrWhiteSpace(notificationId))
    {
        await _cache.DeleteDataAsync(key);
        return Ok("All notifications deleted.");
    }

    // Ako postoji notificationId -> brišemo samo jednu
    var raw = await _cache.ListRangeAsync(key, 0, -1);
    if (raw.Length == 0)
        return NotFound("No notifications found.");

    var jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    var updatedList = new List<string>();

    foreach (var item in raw)
    {
        if (string.IsNullOrWhiteSpace(item))
            continue;

        try
        {
            var dto = JsonSerializer.Deserialize<NottificationDTO>(item, jsonOptions);
            if (dto != null && dto.id != notificationId)
            {
                updatedList.Add(item);
            }
        }
        catch
        {
            // preskoči nevalidan JSON
        }
    }

    // Obriši staru listu
    await _cache.DeleteDataAsync(key);

    // Upisi nazad filtriranu listu
    foreach (var item in updatedList)
    {
        await _cache.ListRightPushAsync(key, item);
    }

    return Ok("Notification deleted.");
}
}