using Microsoft.AspNetCore.Mvc;
using WebApplication1.Admin.Repository.Query;
using WebApplication1.Admin.DTOs;

namespace WebApplication1.Admin.Controllers;

public class TutorController : ControllerBase
{
    private readonly TutorQuery tutorQuery;
    private readonly TutorTimeSlotQuery tutorTimeSlotQuery;

    public TutorController()
    {
        tutorQuery = new TutorQuery();
        tutorTimeSlotQuery = new TutorTimeSlotQuery();
    }

    [HttpGet]
    public async Task<IActionResult> GetList(int offset, int limit)
    {
        var tutors = await tutorQuery.GetList(null, offset, limit);

        var response = new
        {
            data = tutors,
        };
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetListTimeFree(long? tutorId)
    {
        if (!tutorId.HasValue)
        {
            return BadRequest(new { error = "TutorId is required" });
        }

        try
        {
            var timeSlots = await tutorTimeSlotQuery.GetTutorPersonalTime(tutorId.Value);
            
            var response = new
            {
                data = timeSlots,
                message = "Success"
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", details = ex.Message });
        }
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidateTutorSlotAttribute))]
    public async Task<IActionResult> AddTimeFree(long? tutorId, [FromBody] TutorTimeSlotCreateDTO request)
    {
        try
        {
            var result = await tutorTimeSlotQuery.AddTutorTime(
                tutorId.Value,
                request.Date,
                request.StartTime,
                request.EndTime
            );

            if (result)
            {
                return Ok(new { message = "Time slots added successfully" });
            }
            else
            {
                return StatusCode(500, new { error = "Failed to add time slots" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", details = ex.Message });
        }
    }

    [HttpPut]
    [ServiceFilter(typeof(ValidateTutorSlotAttribute))]
    public async Task<IActionResult> ChangeTimeFree(long? tutorId, [FromBody] TutorTimeSlotCreateDTO request)
    {
        if (!tutorId.HasValue)
        {
            return BadRequest(new { error = "TutorId is required" });
        }

        if (request == null)
        {
            return BadRequest(new { error = "Request body is required" });
        }

        try
        {
            var result = await tutorTimeSlotQuery.ChangeTutorTime(
                tutorId.Value, 
                request.Date, 
                request.StartTime, 
                request.EndTime
            );

            if (result)
            {
                return Ok(new { message = "Time slots changed successfully" });
            }
            else
            {
                return StatusCode(500, new { error = "Failed to change time slots" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", details = ex.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTimeFree(long? pTasId)
    {
        try
        {
            var result = await tutorTimeSlotQuery.DeleteTutorTime(
                pTasId.Value
            );

            if (result)
            {
                return Ok(new { message = "Time slots deleted successfully" });
            }
            else
            {
                return StatusCode(500, new { error = "Failed to deleted time slots" });
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = "Internal server error", details = e.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UnconfirmTimeFree(long? pTasId)
    {
        try
        {
            var result = await tutorTimeSlotQuery.UnconfirmTutorTime(
                pTasId.Value
            );

            if (result)
            {
                return Ok(new { message = "Time slots unconfirm successfully" });
            }
            else
            {
                return StatusCode(500, new { error = "Failed to unconfirm time slots" });
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = "Internal server error", details = e.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> ConfirmTimeFree(long? pTasId)
    {
        try
        {
            var result = await tutorTimeSlotQuery.ConfirmTutorTime(
                pTasId.Value
            );

            if (result)
            {
                return Ok(new { message = "Time slots confirm successfully" });
            }
            else
            {
                return StatusCode(500, new { error = "Failed to confirm time slots" });
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = "Internal server error", details = e.Message });
        }
    }
}