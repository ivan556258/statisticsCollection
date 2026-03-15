using System.Threading;
using Microsoft.Extensions.Logging;
using WebApplication1.Admin.DTOs;
using WebApplication1.Admin.Repository.Query;

namespace WebApplication1.Admin.Services;

public enum ChangeTutorTimezoneError
{
    None,
    TimezoneNotFound,
    TutorNotFound,
    DatabaseError
}

public class ChangeTutorTimezoneResponse
{
    public bool Success => Error == ChangeTutorTimezoneError.None;
    public ChangeTutorTimezoneError Error { get; init; }
}

public class TimezoneService
{
    private readonly TimezoneQuery _timezoneQuery;
    private readonly ILogger<TimezoneService> _logger;

    public TimezoneService(TimezoneQuery timezoneQuery, ILogger<TimezoneService> logger)
    {
        _timezoneQuery = timezoneQuery;
        _logger = logger;
    }

    public async Task<IReadOnlyList<TimezoneDTO>> SearchTimezonesAsync(string? query, int limit, CancellationToken cancellationToken)
    {
        try
        {
            return await _timezoneQuery.SearchAsync(query, limit, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while searching timezones. Query: {Query}, Limit: {Limit}", query, limit);
            throw;
        }
    }

    public async Task<ChangeTutorTimezoneResponse> ChangeTutorTimezoneAsync(long tutorId, string timezone, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _timezoneQuery.ChangeTutorTimezoneAsync(tutorId, timezone, cancellationToken);

            return result switch
            {
                ChangeTutorTimezoneResult.Success => new ChangeTutorTimezoneResponse
                {
                    Error = ChangeTutorTimezoneError.None
                },
                ChangeTutorTimezoneResult.TimezoneNotFound => new ChangeTutorTimezoneResponse
                {
                    Error = ChangeTutorTimezoneError.TimezoneNotFound
                },
                ChangeTutorTimezoneResult.TutorNotFound => new ChangeTutorTimezoneResponse
                {
                    Error = ChangeTutorTimezoneError.TutorNotFound
                },
                _ => new ChangeTutorTimezoneResponse
                {
                    Error = ChangeTutorTimezoneError.DatabaseError
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while changing tutor timezone. TutorId: {TutorId}, Timezone: {Timezone}", tutorId, timezone);
            return new ChangeTutorTimezoneResponse
            {
                Error = ChangeTutorTimezoneError.DatabaseError
            };
        }
    }
}

