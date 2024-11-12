namespace WebApplication1.Statistics.Services;

public class TrackerServices(IHttpContextAccessor httpContextAccessor)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetExternalIpAddress()
    {
        string? ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        if (_httpContextAccessor.HttpContext?.Request.Headers.ContainsKey("X-Forwarded-For") == true)
        {
            ipAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        }

        return ipAddress ?? "IP адрес не найден";
    }

    public string GetReferrerUrl()
    {
        string? referer = _httpContextAccessor.HttpContext?.Request.Headers["Referer"].ToString();

        if (referer == null || string.IsNullOrEmpty(referer))
        {
            return "Реферер не найден или отсутствует";
        }

        return referer;
    }

    private static string GetIpv4FromIPv6(string ipv6)
    {
        if (ipv6.StartsWith("::ffff:"))
        {
            return ipv6.Substring(7);
        }

        return ipv6;
    }
}