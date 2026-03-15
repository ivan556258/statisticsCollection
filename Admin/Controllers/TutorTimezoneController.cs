using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Admin.DTOs;
using WebApplication1.Admin.Services;

namespace WebApplication1.Admin.Controllers;

[ApiController]
[Route("api/v1/tutor/timezone")]
public class TutorTimezoneController : ControllerBase
{
    private readonly TimezoneService _timezoneService;

    public TutorTimezoneController(TimezoneService timezoneService)
    {
        _timezoneService = timezoneService;
    }

    [HttpPost]
    public async Task<IActionResult> Change([FromBody] ChangeTutorTimezoneRequestDTO request, CancellationToken cancellationToken = default)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Timezone))
        {
            return BadRequest(new { error = "Timezone is required" });
        }

        // Пока авторизация не настроена — поддерживаем "колхозный" режим:
        // 1) пробуем взять tutor_id из токена
        // 2) если токена нет, берём tutor_id из body
        var tutorId = GetTutorIdFromClaims(User) ?? request.TutorId;
        if (tutorId is null || tutorId <= 0)
        {
            return BadRequest(new { error = "TutorId is required (temporary mode: pass tutor_id in body)" });
        }

        var result = await _timezoneService.ChangeTutorTimezoneAsync(tutorId.Value, request.Timezone, cancellationToken);

        return result.Error switch
        {
            ChangeTutorTimezoneError.None => Ok(new { success = true }),
            ChangeTutorTimezoneError.TimezoneNotFound => BadRequest(new { error = "Timezone not found" }),
            ChangeTutorTimezoneError.TutorNotFound => NotFound(new { error = "Tutor not found" }),
            ChangeTutorTimezoneError.DatabaseError => StatusCode(500, new { error = "Database error while changing tutor timezone" }),
            _ => StatusCode(500, new { error = "Unknown error while changing tutor timezone" })
        };
    }

    private static long? GetTutorIdFromClaims(ClaimsPrincipal user)
    {
        var tutorIdClaim =
            user.FindFirst("tutor_id") ??
            user.FindFirst("tutorId") ??
            user.FindFirst(ClaimTypes.NameIdentifier);

        if (tutorIdClaim == null)
        {
            return null;
        }

        return long.TryParse(tutorIdClaim.Value, out var tutorId)
            ? tutorId
            : null;
    }
}

