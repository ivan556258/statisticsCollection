using MySql.Data.MySqlClient;
using WebApplication1.Admin.DTOs;
using WebApplication1.DatabaseManager;

namespace WebApplication1.Admin.Repository.Query;

public class TutorQuery
{
    private readonly MysqlManager mysqlManager;

    public TutorQuery()
    {
        mysqlManager = MysqlManager.Instance;
    }
    
        public async Task<List<TutorDTO>> GetList(string? code, int offset, int limit)
    {
        List<TutorDTO> tutors = new List<TutorDTO>();
        var results = new List<Dictionary<string, object>>();
        using (var connection = MysqlManager.GetConnection())
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = new MySqlCommand("tutors_list_filter", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_code", code);
                    command.Parameters.AddWithValue("@p_limit", limit);
                    command.Parameters.AddWithValue("@p_offset", offset);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var tutor = new TutorDTO()
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"] as string,
                                Photo = reader["photo"] as string,
                                Code = reader["code"] as string,
                            };
                            tutors.Add(tutor);
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

        return tutors;
    }
}