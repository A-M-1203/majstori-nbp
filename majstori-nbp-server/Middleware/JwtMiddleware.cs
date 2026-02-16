using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using majstori_nbp_server.Services;

namespace majstori_nbp_server.Middleware;

public class JwtAuthorizeFilter : IAsyncAuthorizationFilter
{
    private readonly ICacheService _cache;

    public JwtAuthorizeFilter(ICacheService cache)
    {
        _cache = cache;
    }

    private record SessionData(string userId, string role);

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var auth = context.HttpContext.Request.Headers["Authorization"].ToString();

        

        var token = auth.Trim();

        var json = await _cache.GetStringAsync($"session:{token}");
        if (string.IsNullOrEmpty(json))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        SessionData? session;
        try
        {
            session = JsonSerializer.Deserialize<SessionData>(json);
        }
        catch
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (session?.userId is null || session.role is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["userId"] = session.userId;
        context.HttpContext.Items["role"] = session.role;

        // opciono: “sliding expiration”
        await _cache.SetKeyExpiryTimeAsync($"session:{token}", TimeSpan.FromMinutes(30));
    }
}
