using WebApplication1.Statistics.DTOs;
using WebApplication1.Statistics.RepositoryHttp.Interfaces;

namespace WebApplication1.Statistics.RepositoryHttp.Queries;

public class PosthogQuery : PosthogInterface
{
    public async Task SendEvent(PosthogHttpDto posthogHttpDto)
    {
        await BuildHttp(posthogHttpDto);   
    }

    protected async Task BuildHttp(PosthogHttpDto posthogHttpDto)
    {
        using var client = new HttpClient();
    
        var json = $@"
    {{
        ""api_key"": ""{posthogHttpDto.ApiKey}"", 
        ""event"": ""{posthogHttpDto.Name}"",
        ""properties"": {{
            ""distinct_id"": 1,
            ""theme"": ""{posthogHttpDto.Name}"",
            ""from"": ""{posthogHttpDto.Referrer}"",
            ""to"": ""tg""
        }},
        ""timestamp"": ""{posthogHttpDto.Timestamp}""
    }}";

        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
    
        var response = await client.PostAsync($"{posthogHttpDto.Url}/capture/", content);
        response.EnsureSuccessStatusCode();
    }
}