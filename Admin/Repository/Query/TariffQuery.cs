using MySql.Data.MySqlClient;
using WebApplication1.Admin.DTOs;
using WebApplication1.DatabaseManager;

namespace WebApplication1.Admin.Repository.Query;

public class TariffQuery
{
    private readonly MysqlManager mysqlManager;

    public TariffQuery()
    {
        mysqlManager = MysqlManager.Instance;
    }

    public async Task<List<TariffDTO>> GetList()
    {
        List<TariffDTO> tariffs = new List<TariffDTO>();

        using var connection = MysqlManager.GetConnection();
        try
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            using var command = new MySqlCommand("tariff_list", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var tariff = new TariffDTO()
                {
                    Id = reader["id"] is ulong idVal ? idVal : null,
                    Name = reader["name"] as string,
                    Price = reader["price"] as int? ?? Convert.ToInt32(reader["price"]),
                    PricePerYear = reader["price_per_year"] as int? ?? Convert.ToInt32(reader["price_per_year"]),
                    Discount = reader["discount"] as int? ?? Convert.ToInt32(reader["discount"]),
                    Days = reader["days"] as int? ?? Convert.ToInt32(reader["days"]),
                    IsTurnOn = Convert.ToBoolean(reader["is_turn_on"]),
                    CreatedAt = reader["created_at"] as DateTime?,
                    UpdatedAt = reader["updated_at"] as DateTime?,
                    PricePerMonth = reader["price_per_month"] as int? ?? Convert.ToInt32(reader["price_per_month"])
                };

                tariffs.Add(tariff);
            }
        }
        finally
        {
            if (connection.State == System.Data.ConnectionState.Open)
                await connection.CloseAsync();
        }

        return tariffs;
    }
}
