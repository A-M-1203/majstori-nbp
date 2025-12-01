using majstori_nbp_server.DTOs.AuthDTOs;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace majstori_nbp_server.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IKlijentService _klijentService;
    private readonly IAuthService _authService;

    public AuthController(IKlijentService klijentService, IAuthService authService)
    {
        _klijentService = klijentService;
        _authService = authService;
    }

    [HttpPost(ApiEndpoints.V1.Auth.RegisterMajstor)]
    public async Task<ActionResult> RegisterMajstor(RegisterMajstorDTO registerRequest)
    {
        var response = await _authService.RegisterMajstorAsync(registerRequest);
        if (response is null)
        {
            return BadRequest("Korisnik vec postoji");
        }

        return Ok(response);
    }

    [HttpPost(ApiEndpoints.V1.Auth.RegisterKlijent)]
    public async Task<ActionResult> RegisterKlijent(RegisterKlijentDTO registerRequest)
    {
        var response = await _authService.RegisterKlijentAsync(registerRequest);
        if (response is null)
        {
            return BadRequest("Korisnik vec postoji");
        }

        return Ok(response);
    }

    [HttpPost(ApiEndpoints.V1.Auth.Login)]
    public async Task<ActionResult> Login(LoginDTO loginRequest)
    {
        var user = new UserDTO
        {
            Email = loginRequest.Email,
            Password = loginRequest.Password
        };

        string? userId = await _klijentService.GetUserId(user.Email);
        if (string.IsNullOrEmpty(userId))
        {
            return NotFound("Pogresan email ili password");
        }

        string[] roleAndUserId = userId.Split(":");
        if (roleAndUserId[0] == "klijent")
        {
            var response = await _authService.LoginKlijentAsync(user, roleAndUserId[1]);
            if (response is null)
            {
                return NotFound("Pogresan email ili password");
            }

            return Ok(response);
        }
        else
        {
            var response = await _authService.LoginMajstorAsync(user, roleAndUserId[1]);
            if (response is null)
            {
                return NotFound("Pogresan email ili password");
            }

            return Ok(response);
        }
    }
}