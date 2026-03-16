using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;
using WebApplication1.Admin.DTOs;
using WebApplication1.DatabaseManager;

namespace WebApplication1.Admin.Repository.Query;

public enum ChangeTutorTimezoneResult
{
    Success,
    TimezoneNotFound,
    TutorNotFound
}

public class TimezoneQuery
{
    public async Task<bool> TutorExistsAsync(long tutorId, CancellationToken cancellationToken)
    {
        await using var connection = MysqlManager.GetConnection();

        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            await using var command = new MySqlCommand(
                "SELECT 1 FROM lingua_hub.`user` WHERE id = @p_tutor_id LIMIT 1",
                connection)
            {
                CommandType = CommandType.Text
            };

            command.Parameters.AddWithValue("@p_tutor_id", tutorId);

            var scalar = await command.ExecuteScalarAsync(cancellationToken);
            return scalar != null && scalar != DBNull.Value;
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }
    }

    public async Task<bool> TimezoneExistsAsync(string timezoneName, CancellationToken cancellationToken)
    {
        // Не привязываемся к конкретной таблице/схеме:
        // используем уже существующую SP search_timezones и проверяем точное совпадение name.
        var items = await SearchAsync(timezoneName, limit: 20, cancellationToken);
        return items.Any(x => string.Equals(x.Name, timezoneName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IReadOnlyList<TimezoneDTO>> SearchAsync(string? query, int limit, CancellationToken cancellationToken)
    {
        var result = new List<TimezoneDTO>();

        await using var connection = MysqlManager.GetConnection();

        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            // В MySQL из кода вызываем процедуру без фигурных скобок:
            // CALL lingua_hub.`admin.search_timezones`('Asia', 10);
            await using var command = new MySqlCommand(
                "CALL `admin`.`search_timezones`(@p_query, @p_limit)",
                connection)
            {
                CommandType = CommandType.Text
            };

            command.Parameters.AddWithValue("@p_query", string.IsNullOrWhiteSpace(query) ? (object)DBNull.Value : query);
            command.Parameters.AddWithValue("@p_limit", limit);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                var timezone = new TimezoneDTO
                {
                    Id = Convert.ToInt64(reader["id"]),
                    Name = Convert.ToString(reader["name"]) ?? string.Empty,
                    Label = Convert.ToString(reader["label"]) ?? string.Empty,
                    UtcOffset = Convert.ToString(reader["utc_offset"]) ?? string.Empty
                };

                result.Add(timezone);
            }
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }

        return result;
    }

    public async Task<ChangeTutorTimezoneResult> ChangeTutorTimezoneAsync(long tutorId, string timezone, CancellationToken cancellationToken)
    {
        // Явные проверки, чтобы вернуть корректные 400/404 без зависимости от текста ошибок SP
        if (!await TutorExistsAsync(tutorId, cancellationToken))
        {
            return ChangeTutorTimezoneResult.TutorNotFound;
        }

        if (!await TimezoneExistsAsync(timezone, cancellationToken))
        {
            return ChangeTutorTimezoneResult.TimezoneNotFound;
        }

        await using var connection = MysqlManager.GetConnection();

        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            await using var command = new MySqlCommand(
                "CALL lingua_hub.`admin.tutors_change_timezone`(@p_tutor_id, @p_new_timezone)",
                connection)
            {
                CommandType = CommandType.Text
            };

            command.Parameters.AddWithValue("@p_tutor_id", tutorId);
            command.Parameters.AddWithValue("@p_new_timezone", timezone);

            try
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
                return ChangeTutorTimezoneResult.Success;
            }
            catch (MySqlException ex)
            {
                // Ожидаем, что хранимая процедура в сообщении ошибки укажет причину
                // Например: TIMEZONE_NOT_FOUND или TUTOR_NOT_FOUND
                var message = ex.Message ?? string.Empty;

                if (message.Contains("TIMEZONE_NOT_FOUND", StringComparison.OrdinalIgnoreCase))
                {
                    return ChangeTutorTimezoneResult.TimezoneNotFound;
                }

                if (message.Contains("TUTOR_NOT_FOUND", StringComparison.OrdinalIgnoreCase))
                {
                    return ChangeTutorTimezoneResult.TutorNotFound;
                }

                throw;
            }
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }
    }
}

