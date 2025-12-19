using majstori_nbp_server.DTOs;
using majstori_nbp_server.DTOs.KlijentDTOs;
using majstori_nbp_server.Middleware;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;

namespace majstori_nbp_server.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class korisnikController:ControllerBase
{
    private readonly IKlijentService _klijentService;

    public korisnikController(IKlijentService klijentService)
    {
        _klijentService = klijentService;
    }

      [Microsoft.AspNetCore.Mvc.HttpPost("signup")]
      public async Task<IActionResult> signup([FromBody] CreateKlijentDTO createKlijentDTO)
      {
          Console.WriteLine(createKlijentDTO);
          bool has = await _klijentService.ExistsEmail(createKlijentDTO.email);
          if (has)
          {
              return BadRequest("User with given email already exists");
          }
          bool result = await _klijentService.CreateAsync(createKlijentDTO);
          if (result)
          {
              return Ok("user created");
          }
          else
          {
              return BadRequest("User not created");
          }
      }

      [Microsoft.AspNetCore.Mvc.HttpPost("signin")]
      public async Task<IActionResult> signin([FromBody] LoginDTO loginDTO)
      {
          string token = await _klijentService.SignIn(loginDTO.email, loginDTO.password);
          if (token == "")
          {
              string message = "Log in neuspesan";
              var data = new { message = message };
              return NotFound(new JsonResult(data));
          }
          else
          {
              string message = "Log in uspesan";
              string type = "korisnik";
              var data = new { message = message, type = type, token = token };
              return Ok(new JsonResult(data).Value);
          }
      }

      [HttpGet]
      [Route("profile")]
      [ServiceFilter(typeof(JwtAuthorizeFilter))]
      public async Task<IActionResult> getProfile()
      {
          string? id = HttpContext.Items["userId"] as string;
          string? role = HttpContext.Items["role"] as string;
          if (role ==null || !Authorization.Authorization.IsKorisnik(role))
          {
              return Unauthorized();
          }
          if (id == null)
          {
              return Unauthorized();
          }
          return Ok(await _klijentService.GetByIdAsync(id));
      }
      [HttpGet]
      [Route("{userId}")]
      [ServiceFilter(typeof(JwtAuthorizeFilter))]
      public async Task<IActionResult> getKorisnikInfo(string userId)
      {
          return Ok(await _klijentService.GetByIdAsync(userId));
      }
      [HttpPut]
      [Route("")]
      [ServiceFilter(typeof(JwtAuthorizeFilter))]
      public async Task<IActionResult> update([FromForm] UpdateKlijentDTO updateKlijentDTO)
      {
          string id = HttpContext.Items["userId"] as string;
          string role = HttpContext.Items["role"] as string;
          if (!Authorization.Authorization.IsKorisnik(role))
          {
              return Unauthorized();
          }
          if (id == null)
          {
              return Unauthorized();
          }
          return Ok(await _klijentService.UpdateAsync(id, updateKlijentDTO));
      }
}