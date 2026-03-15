using Microsoft.AspNetCore.Mvc;
using WebApplication1.Admin.Services;

namespace WebApplication1.Admin.Controllers;

[ApiController]
[Route("api/v1/timezones")]
public class TimezonesController : ControllerBase
{
    private readonly TimezoneService _timezoneService;

    public TimezonesController(TimezoneService timezoneService)
    {
        _timezoneService = timezoneService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string? query, int limit = 20, CancellationToken cancellationToken = default)
    {
        if (limit <= 0)
        {
            limit = 20;
        }

        if (limit > 100)
        {
            limit = 100;
        }

        try
        {
            var timezones = await _timezoneService.SearchTimezonesAsync(query, limit, cancellationToken);

            var response = new
            {
                data = timezones
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Database error while searching timezones", details = ex.Message });
        }
    }
}

