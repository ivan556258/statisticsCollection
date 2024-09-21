namespace WebApplication1.DTOs;

public record PosthogHttpDto
{
    public string Url {get; set;}
    public string ApiKey {get; set;}
    public string Referrer {get; set;}
    public string Timestamp {get; set;}
    public string Name {get; set;}
    public string Ip {get; set;}
}