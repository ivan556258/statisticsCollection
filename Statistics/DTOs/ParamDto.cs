namespace WebApplication1.Statistics.DTOs;

public record ParamDto
{
    public string Referrer { get; set; }
    public string ExternalIp { get; set; }
    public string Lang { get; set; }
    public string To { get; set; }
    public string From { get; set; }
}