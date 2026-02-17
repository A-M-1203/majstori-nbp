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

    // Očekuje JSON u Redis-u: {"userId":"...","role":"..."}
    private record SessionData(string userId, string role);

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var auth = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(auth))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Podržava:
        // 1) "Bearer <token>"
        // 2) "<token>" (fallback)
        string token;
        if (auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = auth.Substring("Bearer ".Length).Trim();
        else
            token = auth.Trim();

        if (string.IsNullOrWhiteSpace(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var key = $"session:{token}";

        var json = await _cache.GetStringAsync(key);
        if (string.IsNullOrWhiteSpace(json))
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

        if (session is null ||
            string.IsNullOrWhiteSpace(session.userId) ||
            string.IsNullOrWhiteSpace(session.role))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["userId"] = session.userId;
        context.HttpContext.Items["role"] = session.role;

        // opciono: “sliding expiration”
        await _cache.SetKeyExpiryTimeAsync(key, TimeSpan.FromMinutes(30));
    }
}
