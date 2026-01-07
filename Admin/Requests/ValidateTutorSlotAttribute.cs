using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MySql.Data.MySqlClient;
using System.Data;
using WebApplication1.Admin.DTOs;

public class ValidateTutorSlotAttribute : ActionFilterAttribute
{
    private readonly string _connectionString;

    public ValidateTutorSlotAttribute(string connectionString)
    {
        _connectionString = connectionString;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.TryGetValue("tutorId", out var tutorIdObj))
        {
            context.Result = new BadRequestObjectResult(new { error = "TutorId is required" });
            return;
        }

        long tutorId = 0;
        
        if (tutorIdObj is long directTutorId)
        {
            tutorId = directTutorId;
        }
        else
        {
            context.Result = new BadRequestObjectResult(new { error = "TutorId is required" });
            return;
        }

        if (!context.ActionArguments.TryGetValue("request", out var requestObj) || !(requestObj is TutorTimeSlotCreateDTO request))
        {
            context.Result = new BadRequestObjectResult(new { error = "Request body is required" });
            return;
        }

        if (request.Date.Date < DateTime.UtcNow.Date)
        {
            context.Result = new BadRequestObjectResult(new { error = "Date cannot be in the past" });
            return;
        }

        if (request.Date.Date > DateTime.UtcNow.Date.AddDays(30))
        {
            context.Result = new BadRequestObjectResult(new { error = "Date cannot be more than 30 days in advance" });
            return;
        }

        // Проверка на занятость слота
        var existingSlots = await GetExistingTimeSlotsAsync(tutorId);

        foreach (var slot in existingSlots)
        {
            if (slot.Date?.Date == request.Date.Date &&
                slot.StartTime.HasValue && slot.EndTime.HasValue &&
                (
                    (request.StartTime >= slot.StartTime.Value && request.StartTime < slot.EndTime.Value) ||
                    (request.EndTime > slot.StartTime.Value && request.EndTime <= slot.EndTime.Value) ||
                    (request.StartTime <= slot.StartTime.Value && request.EndTime >= slot.EndTime.Value)
                ))
            {
                context.Result = new BadRequestObjectResult(new { error = "Time slot overlaps with an existing one" });
                return;
            }
        }

        await next();
    }

    private async Task<List<TutorTimeSlotDTO>> GetExistingTimeSlotsAsync(long tutorId)
    {
        var result = new List<TutorTimeSlotDTO>();

        await using var connection = new MySqlConnection(_connectionString);
        await using var command = new MySqlCommand("`admin.tutor_personal_time`", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@p_tutor_id", tutorId);
        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new TutorTimeSlotDTO
            {
                Id = Convert.ToInt64(reader["id"]),
                TutorId = Convert.ToInt64(reader["tutor_id"]),
                Date = Convert.ToDateTime(reader["date"]),
                StartTime = (TimeSpan)reader["start_time"],
                EndTime = (TimeSpan)reader["end_time"],
                StudentID = reader["student_id"] != DBNull.Value ? Convert.ToInt64(reader["student_id"]) : null,
                StudentName = reader["student_name"] != DBNull.Value ? Convert.ToString(reader["student_name"]) : null,
                StudentEmail = reader["student_name"] != DBNull.Value ? Convert.ToString(reader["student_email"]) : null,
                IsConfirm = Convert.ToBoolean(reader["is_confirm"])
            });
        }

        return result;
    }
}
