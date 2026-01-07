using WebApplication1.DatabaseManager;
using MySql.Data.MySqlClient;
using WebApplication1.Admin.Repository.Query;
using System.Collections.Generic;
using System.Data;
using WebApplication1.Admin.DTOs;

namespace WebApplication1.Admin.Repository.Query;

public class StudentQuery
{
    private readonly MysqlManager mysqlManager;

    public StudentQuery()
    {
        mysqlManager = MysqlManager.Instance;
    }

    public async Task<List<StudentDTO>> GetList(int tutorId, int offset, int limit)
    {
        List<StudentDTO> students = new List<StudentDTO>();
        var results = new List<Dictionary<string, object>>();
        using (var connection = MysqlManager.GetConnection())
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = new MySqlCommand("student_list_filter", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tutor_id", tutorId);
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@limit", limit);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var student = new StudentDTO()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"] as string,
                                Email = reader["Email"] as string,
                                TelegramId = Convert.ToInt32(reader["telegram_id"]),
                                TelegramNickname = reader["telegram_nickname"] as string,
                                NextLesson = reader["next_lesson"].ToString() as string,
                                AvailableLessons = reader.IsDBNull(reader.GetOrdinal("available_lessons")) ? 0 : reader.GetInt32(reader.GetOrdinal("available_lessons")),
                                Lessons = reader.IsDBNull(reader.GetOrdinal("lessons")) ? 0 : reader.GetInt32(reader.GetOrdinal("lessons")),
                                Activity = reader.IsDBNull(reader.GetOrdinal("activity")) ? 0 : reader.GetInt32(reader.GetOrdinal("activity"))
                            };
                            students.Add(student);
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
        
        return students;
    }

    public async Task<int> GetListCount(int tutorId)
    {
        int result = 0;
        using (var connection = MysqlManager.GetConnection())
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = new MySqlCommand("student_list_count", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tutor_id", tutorId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int countStudents = Convert.ToInt32(reader["countStudents"]);
                            result = countStudents;
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

        return result;
    }

    public async Task CreateStudent(StudentCreateDTO student)
    {
        using var connection = MysqlManager.GetConnection();
        try
        {
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            using var command = new MySqlCommand("student_insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@name", (student.Name ?? "").Trim());
            command.Parameters.AddWithValue("@telegram_id", student.IdTelegram ?? 0);
            command.Parameters.AddWithValue("@telegram_nickname", (student.NicknameTelegram ?? "").Trim());
            command.Parameters.AddWithValue("@group_telegram", (student.GroupTelegram ?? "").Trim());
            command.Parameters.AddWithValue("@lessons", student.Lessons ?? 0);
            command.Parameters.AddWithValue("@country", (student.Country ?? "").Trim());
            command.Parameters.AddWithValue("@tariff", student.Tariff ?? 0);
            command.Parameters.AddWithValue("@tutor_id", student.TutorId ?? 0);

            await command.ExecuteNonQueryAsync();
        }
        finally
        {
            if (connection.State == System.Data.ConnectionState.Open)
                await connection.CloseAsync();
        }
    }
}