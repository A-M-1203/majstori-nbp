using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

[ApiController]
[Route("[controller]")]
public class imagesController:ControllerBase
{
    [HttpGet("{filename}")]
    public async Task<IActionResult> getImage(string filename)
    {
        var path=Path.Combine(Directory.GetCurrentDirectory(),"images",filename);
        if (System.IO.File.Exists(path))
        {
            byte[]bytes=System.IO.File.ReadAllBytes(path);
            return File(bytes, "image/jpeg");
        }

        return NotFound();
    }
}