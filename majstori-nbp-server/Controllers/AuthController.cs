using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using majstori_nbp_server.DTOs.AuthDTOs;
using majstori_nbp_server.DTOs.KlijentDTOs;
using majstori_nbp_server.DTOs.MajstorDTOs;
using majstori_nbp_server.Mappings;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace majstori_nbp_server.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMajstorService _majstorService;
    private readonly IKlijentService _klijentService;
    private readonly IConfiguration _configuration;

    public AuthController(IMajstorService majstorService, IKlijentService klijentService, IConfiguration configuration)
    {
        _majstorService = majstorService;
        _klijentService = klijentService;
        _configuration = configuration;
    }

    [HttpPost(ApiEndpoints.V1.Auth.RegisterMajstor)]
    public async Task<ActionResult> RegisterMajstor(RegisterMajstorDTO registerRequest)
    {
        UserDTO user = registerRequest.MapToUserDTO();
        var hashedPassword = new PasswordHasher<UserDTO>()
            .HashPassword(user, user.Password);

        var majstor = registerRequest.MapToCreateMajstorDTO(hashedPassword);
        var response = await _majstorService.CreateAsync(majstor);
        if (response is null)
        {
            return BadRequest("Korisnik vec postoji");
        }

        return Ok(response);
    }

    [HttpPost(ApiEndpoints.V1.Auth.RegisterKlijent)]
    public async Task<ActionResult> RegisterKlijent(RegisterKlijentDTO registerRequest)
    {
        UserDTO user = registerRequest.MapToUserDTO();
        var hashedPassword = new PasswordHasher<UserDTO>()
            .HashPassword(user, user.Password);

        var klijent = registerRequest.MapToCreateKlijentDTO(hashedPassword);
        var response = await _klijentService.CreateAsync(klijent);
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
            GetKlijentDTO? klijent = await _klijentService.GetByIdAsync(roleAndUserId[1]);
            if (klijent is null)
            {
                return NotFound("Pogresan email ili password");
            }

            if (new PasswordHasher<UserDTO>().VerifyHashedPassword(user, klijent.PasswordHash, user.Password)
                == PasswordVerificationResult.Failed)
            {
                return NotFound("Pogresan email ili password");
            }

            return Ok(new LoginResponse<GetKlijentDTO>
            {
                User = klijent,
                Token = CreateToken(klijent.Id.ToString(), klijent.Email, roleAndUserId[0])
            });
        }
        else
        {
            GetMajstorDTO? majstor = await _majstorService.GetByIdAsync(roleAndUserId[1]);
            if (majstor is null)
            {
                return NotFound("Pogresan email ili password");
            }

            if (new PasswordHasher<UserDTO>().VerifyHashedPassword(user, majstor.PasswordHash, user.Password)
                == PasswordVerificationResult.Failed)
            {
                return NotFound("Pogresan email ili password");
            }

            return Ok(new LoginResponse<GetMajstorDTO>
            {
                User = majstor,
                Token = CreateToken(majstor.Id.ToString(), majstor.Email, roleAndUserId[0])
            });
        }
    }

    private string CreateToken(string userId, string email, string role)
    {
        var claims = new List<Claim>
        {
            new("id", userId),
            new("email", email),
            new("role", role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:TokenKey")!)
        );

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
            audience: _configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}