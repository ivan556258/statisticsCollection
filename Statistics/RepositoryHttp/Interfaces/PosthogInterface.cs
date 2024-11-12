using WebApplication1.Statistics.DTOs;

namespace WebApplication1.Statistics.RepositoryHttp.Interfaces;

public interface PosthogInterface
{
    Task SendEvent(PosthogHttpDto posthogHttpDto);
}