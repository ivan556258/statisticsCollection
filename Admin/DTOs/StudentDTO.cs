using System.Text.Json.Serialization;

namespace WebApplication1.Admin.DTOs;

public record StudentDTO
{
    [JsonPropertyName("id")] 
    public int? Id { get; init; }
    

    [JsonPropertyName("name")] 
    public string? Name { get; init; }
    

    [JsonPropertyName("next_lesson")] 
    public string? NextLesson { get; init; }
    

    [JsonPropertyName("available_lessons")]
    public Int32? AvailableLessons { get; init; }


    [JsonPropertyName("lessons")]
    public Int32? Lessons { get; init; }

    [JsonPropertyName("activity")] 
    public Int32? Activity { get; init; }    
    
    
    [JsonPropertyName("email")] 
    public string? Email { get; init; }
    

    [JsonPropertyName("telegram_id")] 
    public int? TelegramId { get; init; }
    

    [JsonPropertyName("telegram_nickname")]
    public string? TelegramNickname { get; init; }
}