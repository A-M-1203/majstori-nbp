using System.Security.Claims;
using majstori_nbp_server.Helper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace majstori_nbp_server.Middleware;

public class JwtAuthorizeFilter : IAuthorizationFilter
{
    private readonly JwtSecurityTokenHandlerWrapper _wrapper;

    public JwtAuthorizeFilter(JwtSecurityTokenHandlerWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].ToString();
        token= token.Replace("Bearer ", "");
        if (!token.IsNullOrEmpty())
        {
            try
            {
                var claims = _wrapper.ValidateJwtToken(token);
                var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = claims.FindFirst(ClaimTypes.Role)?.Value;
                context.HttpContext.Items["userId"] = userId;
                context.HttpContext.Items["role"] = role;
                context.HttpContext.Response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                context.HttpContext.Response.StatusCode = 403;
            }
        }
        else
        {
            context.HttpContext.Response.StatusCode = 403;
        }
    }
}