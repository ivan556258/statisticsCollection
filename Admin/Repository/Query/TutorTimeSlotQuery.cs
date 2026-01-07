using MySql.Data.MySqlClient;
using WebApplication1.Admin.DTOs;
using WebApplication1.DatabaseManager;

namespace WebApplication1.Admin.Repository.Query;

public class TutorTimeSlotQuery
{
    private readonly MysqlManager mysqlManager;

    public TutorTimeSlotQuery()
    {
        mysqlManager = MysqlManager.Instance;
    }

    public async Task<List<TutorTimeSlotDTO>> GetTutorPersonalTime(long tutorId)
    {
        List<TutorTimeSlotDTO> timeSlots = new List<TutorTimeSlotDTO>();

        using (var connection = MysqlManager.GetConnection())
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = new MySqlCommand("`admin.tutor_personal_time`", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_tutor_id", tutorId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var timeSlot = new TutorTimeSlotDTO()
                            {
                                Id = Convert.ToInt64(reader["id"]),
                                TutorId = Convert.ToInt64(reader["tutor_id"]),
                                Date = Convert.ToDateTime(reader["date"]),
                                StartTime = (TimeSpan)reader["start_time"],
                                EndTime = (TimeSpan)reader["end_time"],
                                StudentID = reader["student_id"] != DBNull.Value
                                    ? Convert.ToInt64(reader["student_id"])
                                    : null,
                                StudentName = reader["student_name"] != DBNull.Value
                                    ? Convert.ToString(reader["student_name"])
                                    : null,
                                StudentEmail = reader["student_name"] != DBNull.Value
                                    ? Convert.ToString(reader["student_email"])
                                    : null,
                                IsConfirm = Convert.ToBoolean(reader["is_confirm"])
                            };
                            timeSlots.Add(timeSlot);
                        }
                    }
                }
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }

        return timeSlots;
    }

    public async Task<bool> AddTutorTime(long tutorId, DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        using (var connection = MysqlManager.GetConnection())
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = new MySqlCommand("`admin.tutors_add_time`", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_tutor_id", tutorId);
                    command.Parameters.AddWithValue("@p_date", date.Date);
                    command.Parameters.AddWithValue("@p_start", startTime);
                    command.Parameters.AddWithValue("@p_end", endTime);

                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error adding tutor time: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }
    }

    public async Task<bool> ChangeTutorTime(long tutorId, DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        using (var connection = MysqlManager.GetConnection())
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command =
                       new MySqlCommand(
                           "{ CALL mojo_staging.`admin.tutors_change_time`(@p_tutor_id, @p_date, @p_start, @p_end) }",
                           connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@p_tutor_id", tutorId);
                    command.Parameters.AddWithValue("@p_date", date.Date);
                    command.Parameters.AddWithValue("@p_start", startTime);
                    command.Parameters.AddWithValue("@p_end", endTime);

                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error changing tutor time: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }
    }

    public async Task<bool> DeleteTutorTime(long pTasId)
    {
        using (var connection = MysqlManager.GetConnection())
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = new MySqlCommand("`admin.tutor_personal_time_delete`", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_tas_id", pTasId);

                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error adding tutor time: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
    
    public async Task<bool> UnconfirmTutorTime(long pTasId)
    {
        using (var connection = MysqlManager.GetConnection())
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = new MySqlCommand("`admin.tutor_personal_time_unconfirm`", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_tas_id", pTasId);

                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error adding tutor time: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
    
    public async Task<bool> ConfirmTutorTime(long pTasId)
    {
        using (var connection = MysqlManager.GetConnection())
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = new MySqlCommand("`admin.tutor_personal_time_confirm`", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_tas_id", pTasId);

                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error adding tutor time: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
}