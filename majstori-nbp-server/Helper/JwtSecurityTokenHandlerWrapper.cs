using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace majstori_nbp_server.Helper;

public class JwtSecurityTokenHandlerWrapper
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public JwtSecurityTokenHandlerWrapper()
    {
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }
    
    public string GenerateJwtToken(string userId, string role)
    {
       
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!);
        
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role)
            };
        
        var identity = new ClaimsIdentity(claims);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            Subject = identity,
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        
        return _jwtSecurityTokenHandler.WriteToken(token);
    }
    
    public ClaimsPrincipal ValidateJwtToken(string token)
    {
      
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!);

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);
            
            return claimsPrincipal;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new ApplicationException("Token has expired.");
        }
    }
}