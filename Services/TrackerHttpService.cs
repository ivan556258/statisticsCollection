using dotenv.net;
using WebApplication1.DTOs;
using WebApplication1.RepositoryHttp.Interfaces;

namespace WebApplication1.Services;

public class TrackerHttpService
{
    private static readonly string? ApiKey;
    private static readonly string? Url;
    private static readonly string Timestamp;

    protected readonly PosthogInterface Posthog;

    public TrackerHttpService(PosthogInterface posthog)
    {
        Posthog = posthog;
    }

    static TrackerHttpService()
    {
        DotEnv.Load();
        ApiKey = Environment.GetEnvironmentVariable("POSTHOG_API_KEY");
        Url = Environment.GetEnvironmentVariable("POSTHOG_API_URL");
        Timestamp = DateTime.Now.ToString("yyyy-MM-dd");
    }

    public void SetStat(ParamDto paramDto)
    {
        PosthogHttpDto posthogHttpDto = new PosthogHttpDto
        {
            Url = Url,
            ApiKey = ApiKey,
            Referrer = paramDto.Referrer,
            Timestamp = Timestamp,
            Name = paramDto.From + "_" + paramDto.Lang,
            Ip = paramDto.ExternalIp
        };
        Posthog.SendEvent(posthogHttpDto);
    }
}