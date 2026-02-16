using majstori_nbp_server.DTOs.MessageDTO;
using majstori_nbp_server.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

[ApiController]
[Route("[controller]")]
public class messageController:ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> getMessages(string id)
    {
        //Ovo je id chata
        throw new NotImplementedException();
    }

    [HttpPost("")]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<IActionResult> sendMessage([FromBody] CreateMessageDTO message)
    {
        message.datum=DateTime.Now;
        message.korisnik=HttpContext.Items["userId"] as string;
        throw new NotImplementedException();
    }
}