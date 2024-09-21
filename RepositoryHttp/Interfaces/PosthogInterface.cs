using WebApplication1.DTOs;

namespace WebApplication1.RepositoryHttp.Interfaces;

public interface PosthogInterface
{
    Task SendEvent(PosthogHttpDto posthogHttpDto);
}