using System.Text.Json.Serialization;

namespace WebApplication1.Admin.DTOs;

public class TutorDTO
{
    [JsonPropertyName("id")] 
    public int? Id { get; init; }
     
    [JsonPropertyName("name")] 
    public string? Name { get; init; }
    
    [JsonPropertyName("photo")] 
    public string? Photo { get; init; }
    
    [JsonPropertyName("code")] 
    public string? Code { get; init; }
}