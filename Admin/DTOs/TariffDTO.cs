using System.Text.Json.Serialization;

namespace WebApplication1.Admin.DTOs;

public class TariffDTO
{
    [JsonPropertyName("id")]
    public ulong? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("price")]
    public int? Price { get; init; }

    [JsonPropertyName("price_per_year")]
    public int? PricePerYear { get; init; }

    [JsonPropertyName("discount")]
    public int? Discount { get; init; }

    [JsonPropertyName("days")]
    public int? Days { get; init; }

    [JsonPropertyName("is_turn_on")]
    public bool? IsTurnOn { get; init; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; init; }

    [JsonPropertyName("price_per_month")]
    public int? PricePerMonth { get; init; }
}