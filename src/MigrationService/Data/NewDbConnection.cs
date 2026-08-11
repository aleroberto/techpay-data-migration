using Microsoft.Data.SqlClient;

namespace MigrationService.Data;

public class NewDbConnection
{
    private readonly string _connectionString;

    public NewDbConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}