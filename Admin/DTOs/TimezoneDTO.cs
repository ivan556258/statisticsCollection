using System.Text.Json.Serialization;

namespace WebApplication1.Admin.DTOs;

public class TimezoneDTO
{
    [JsonPropertyName("id")] 
    public long Id { get; init; }
    
    [JsonPropertyName("name")] 
    public string Name { get; init; } = string.Empty;
    
    [JsonPropertyName("label")] 
    public string Label { get; init; } = string.Empty;
    
    [JsonPropertyName("utc_offset")] 
    public string UtcOffset { get; init; } = string.Empty;
}

