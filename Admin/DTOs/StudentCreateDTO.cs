using System.Text.Json.Serialization;

namespace WebApplication1.Admin.DTOs;

public record StudentCreateDTO
{
    [JsonPropertyName("id")] 
    public int? Id { get; init; }
    

    [JsonPropertyName("name")] 
    public string? Name { get; init; }
    
    [JsonPropertyName("nicknameTelegram")] 
    public string? NicknameTelegram { get; init; }
    
    [JsonPropertyName("idTelegram")] 
    public Int128? IdTelegram { get; init; }
    
    [JsonPropertyName("groupTelegram")] 
    public string? GroupTelegram { get; init; }
    
    [JsonPropertyName("lessons")] 
    public int? Lessons { get; init; }
    
    [JsonPropertyName("country")] 
    public string? Country { get; init; }
    
    [JsonPropertyName("tariff")] 
    public Int128? Tariff { get; init; }
    
    [JsonPropertyName("tutorId")] 
    public int? TutorId { get; init; } 
}