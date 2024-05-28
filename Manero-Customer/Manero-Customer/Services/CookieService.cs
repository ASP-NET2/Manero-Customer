namespace Manero_Customer.Services;

public class CookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetSessionIdCookie(string sessionId)
    {
        CookieOptions options = new()
        {
            Expires = DateTime.Now.AddHours(24),
            HttpOnly = true,
            Secure = true
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append("SessionId", sessionId, options);
    }

    public string GetSessionIdCookie()
    {
        _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("SessionId", out string sessionId);
        return sessionId;
    }
}
