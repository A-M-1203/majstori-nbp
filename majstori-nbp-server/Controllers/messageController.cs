using majstori_nbp_server.DTOs.MessageDTO;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

[ApiController]
[Route("[controller]")]
public class messageController
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> getMessages(string id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> sendMessage([FromBody] CreateMessageDTO message)
    {
        throw new NotImplementedException();
    }
}