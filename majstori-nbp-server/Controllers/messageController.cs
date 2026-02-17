using System.Text.Json;
using majstori_nbp_server.DTOs.MessageDTO;
using majstori_nbp_server.Helper;
using majstori_nbp_server.Middleware;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

[ApiController]
[Route("[controller]")]
public class messageController : ControllerBase
{
    private readonly ICacheService _cache;
    private const int MaxMessages = 200;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = null
    };

    public messageController(ICacheService cache)
    {
        _cache = cache;
    }

    private static string MessagesKey(string chatId) => $"chat:{chatId}:messages";

    // GET /message/{id}  (id = chatId)
    [HttpGet("{id}")]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<IActionResult> getMessages(string id)
    {
        var key = MessagesKey(id);

        // uzmi poslednjih MaxMessages
        var raw = await _cache.ListRangeAsync(key, -MaxMessages, -1);

        var list = new List<CreateMessageDTO>();
        foreach (var item in raw)
        {
            try
            {
                var msg = JsonSerializer.Deserialize<CreateMessageDTO>(item, JsonOpts);
                if (msg != null) list.Add(msg);
            }
            catch { /* preskoči loš zapis */ }
        }

        return Ok(list);
    }

    // POST /message
    [HttpPost("")]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<IActionResult> sendMessage([FromBody] CreateMessageDTO message)
    {
        var userId = HttpContext.Items["userId"] as string;
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        if (message == null || string.IsNullOrWhiteSpace(message.chat) || string.IsNullOrWhiteSpace(message.sadrzaj))
            return BadRequest(new { message = "chat i sadrzaj su obavezni" });

        // server postavlja ove vrednosti
        message._id = Ulid.NewUlid().ToString();
        message.korisnik = userId;
        message.datum = DateTime.UtcNow;

        var json = JsonSerializer.Serialize(message, JsonOpts);
        var key = MessagesKey(message.chat);

        await _cache.ListRightPushAsync(key, json);
        await _cache.ListTrimAsync(key, -MaxMessages, -1);

        return Ok(message);
    }
}
