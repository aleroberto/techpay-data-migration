using Microsoft.Data.SqlClient;

namespace MigrationService.Data;

public class LegacyDbConnection
{
    private readonly string _connectionString;

    public LegacyDbConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}