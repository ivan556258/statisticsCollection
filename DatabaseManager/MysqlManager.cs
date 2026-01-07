using System.Text;
using MySql.Data.MySqlClient;

namespace WebApplication1.DatabaseManager;

using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

public class MysqlManager
{
    private static MysqlManager _instance;
    private static readonly object _lock = new object();
    private readonly MySqlConnection connection;
    // private static readonly string ConnectionString = "server=lh_mysql;port=3306;user=root;password=4321;database=mojo_staging";
    private static readonly string ConnectionString = $"server={Environment.GetEnvironmentVariable("DB_SERVER")};port={Environment.GetEnvironmentVariable("DB_PORT")};user={Environment.GetEnvironmentVariable("DB_USER")};password={Environment.GetEnvironmentVariable("DB_PASSWORD")};database={Environment.GetEnvironmentVariable("DB_DATABASE")}";

    private MysqlManager()
    {
        // Инициализируем соединение при создании экземпляра
        // connection = new MySqlConnection("server=lh_mysql;port=3306;user=root;password=4321;database=mojo_staging");
        connection = new MySqlConnection($"server={Environment.GetEnvironmentVariable("DB_SERVER")};port={Environment.GetEnvironmentVariable("DB_PORT")};user={Environment.GetEnvironmentVariable("DB_USER")};password={Environment.GetEnvironmentVariable("DB_PASSWORD")};database={Environment.GetEnvironmentVariable("DB_DATABASE")}");
        connection.Open();
    }

    public static MysqlManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MysqlManager();
                    }
                }
            }
            return _instance;
        }
    }

    public static MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}

