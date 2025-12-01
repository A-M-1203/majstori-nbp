using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using majstori_nbp_server.DTOs.AuthDTOs;
using majstori_nbp_server.DTOs.KlijentDTOs;
using majstori_nbp_server.DTOs.MajstorDTOs;
using majstori_nbp_server.Mappings;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace majstori_nbp_server.Implementations;

public class AuthService : IAuthService
{
    private readonly IMajstorService _majstorService;
    private readonly IKlijentService _klijentService;
    private readonly IConfiguration _configuration;

    public AuthService(IMajstorService majstorService, IKlijentService klijentService, IConfiguration configuration)
    {
        _majstorService = majstorService;
        _klijentService = klijentService;
        _configuration = configuration;
    }

    public async Task<LoginResponse<GetMajstorDTO>?> LoginMajstorAsync(UserDTO user, string userId)
    {
        GetMajstorDTO? majstor = await _majstorService.GetByIdAsync(userId);
        if (majstor is null)
        {
            return null;
        }

        if (new PasswordHasher<UserDTO>().VerifyHashedPassword(user, majstor.PasswordHash, user.Password)
            == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new LoginResponse<GetMajstorDTO>
        {
            User = majstor,
            Token = CreateToken(majstor.Id.ToString(), majstor.Email, "majstor")
        };
    }

    public async Task<LoginResponse<GetKlijentDTO>?> LoginKlijentAsync(UserDTO user, string userId)
    {
        GetKlijentDTO? klijent = await _klijentService.GetByIdAsync(userId);
        if (klijent is null)
        {
            return null;
        }

        if (new PasswordHasher<UserDTO>().VerifyHashedPassword(user, klijent.PasswordHash, user.Password)
            == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new LoginResponse<GetKlijentDTO>
        {
            User = klijent,
            Token = CreateToken(klijent.Id.ToString(), klijent.Email, "klijent")
        };
    }

    public async Task<GetMajstorDTO?> RegisterMajstorAsync(RegisterMajstorDTO registerRequest)
    {
        UserDTO user = registerRequest.MapToUserDTO();
        var hashedPassword = new PasswordHasher<UserDTO>()
            .HashPassword(user, user.Password);

        var majstor = registerRequest.MapToCreateMajstorDTO(hashedPassword);
        return await _majstorService.CreateAsync(majstor);
    }

    public async Task<GetKlijentDTO?> RegisterKlijentAsync(RegisterKlijentDTO registerRequest)
    {
        UserDTO user = registerRequest.MapToUserDTO();
        var hashedPassword = new PasswordHasher<UserDTO>()
            .HashPassword(user, user.Password);

        var klijent = registerRequest.MapToCreateKlijentDTO(hashedPassword);
        return await _klijentService.CreateAsync(klijent);
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
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}