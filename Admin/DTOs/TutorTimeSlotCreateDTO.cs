using System.Text.Json.Serialization;

namespace WebApplication1.Admin.DTOs;

public class TutorTimeSlotCreateDTO
{
    [JsonPropertyName("date")] 
    public DateTime Date { get; set; }
    
    [JsonPropertyName("start_time")] 
    public TimeSpan StartTime { get; set; }
    
    [JsonPropertyName("end_time")] 
    public TimeSpan EndTime { get; set; }
} 