namespace Manero_Customer.Services;

public class SessionIdMiddleware
{
    private readonly RequestDelegate _next;

    public SessionIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Cookies.ContainsKey("SessionId"))
        {
            var sessionId = Guid.NewGuid().ToString();
            context.Response.Cookies.Append("SessionId", sessionId, new CookieOptions
            {
                Expires = DateTime.Now.AddHours(24),
                HttpOnly = true,
                Secure = context.Request.IsHttps
            });
            Console.WriteLine($"SessionId cookie set: {sessionId}");
        }

        await _next(context);
    }
}


