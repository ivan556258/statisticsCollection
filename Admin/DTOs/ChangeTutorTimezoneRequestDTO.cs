using System.Text.Json.Serialization;

namespace WebApplication1.Admin.DTOs;

public class ChangeTutorTimezoneRequestDTO
{
    // Временный "колхозный" режим (пока не настроена авторизация):
    // можно передать tutor_id прямо в body.
    [JsonPropertyName("tutor_id")]
    public long? TutorId { get; init; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; init; } = string.Empty;
}

