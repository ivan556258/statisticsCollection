using System.Text.Json.Serialization;

namespace WebApplication1.Admin.DTOs;

public class TutorTimeSlotDTO
{
    [JsonPropertyName("id")] 
    public long? Id { get; init; }
    
    [JsonPropertyName("tutor_id")] 
    public long? TutorId { get; init; }
     
    [JsonPropertyName("date")] 
    public DateTime? Date { get; init; }
    
    [JsonPropertyName("start_time")] 
    public TimeSpan? StartTime { get; init; }
    
    [JsonPropertyName("end_time")] 
    public TimeSpan? EndTime { get; init; }
    
    [JsonPropertyName("student_id")] 
    public long? StudentID { get; init; }
    
    [JsonPropertyName("student_name")] 
    public string? StudentName { get; init; }
    
    [JsonPropertyName("student_email")] 
    public string? StudentEmail { get; init; }
    
    [JsonPropertyName("is_confirm")] 
    public bool? IsConfirm { get; init; }
} 