using majstori_nbp_server.DTOs.NottificationDTOs;
using majstori_nbp_server.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

[ApiController]
[Route("[controller]")]
public class nottificationController:ControllerBase
{
    [HttpGet]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<ActionResult> Get()
    {
        string id = HttpContext.Request.Headers["id"];
        List<NottificationDTO>list=new List<NottificationDTO>();
        list.Add(new NottificationDTO()
        {
            id="1", 
            text= "ana_likes liked your photo.", 
            time= DateTime.Now, 
            avatarUrl= "https://i.pravatar.cc/64?img=5"
        });
        list.Add(new NottificationDTO()
        {
            id="1", 
            text= "jelena started following you.", 
            time= DateTime.Now,
            avatarUrl= "https://i.pravatar.cc/64?img=5"
        });
        list.Add(new NottificationDTO()
        {
            id="1", 
            text= "marko99 commented", 
            time= DateTime.Now,
            avatarUrl= "https://i.pravatar.cc/64?img=5"
        });
        return Ok(list);
    }

    [HttpDelete("")]
    [ServiceFilter(typeof(JwtAuthorizeFilter))]
    public async Task<IActionResult> Delete()
    {
        string id = HttpContext.Request.Headers["id"];
        throw new NotImplementedException();
    }
    
}