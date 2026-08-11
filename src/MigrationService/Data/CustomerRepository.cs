using Microsoft.Data.SqlClient;

namespace MigrationService.Data;

public class CustomerRepository
{
    private readonly string _connectionString;

    public CustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void ListCustomers()
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        const string sql = """
            SELECT CustomerId, Name, Document, Email
            FROM Customers
            ORDER BY CustomerId;
            """;

        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine(
                $"{reader["CustomerId"]} - " +
                $"{reader["Name"]} - " +
                $"{reader["Document"]} - " +
                $"{reader["Email"]}");
        }
    }

    public void MigrateCustomers(
        string targetConnectionString)
    {
        using var sourceConnection = new SqlConnection(_connectionString);
        using var targetConnection = new SqlConnection(targetConnectionString);

        sourceConnection.Open();
        targetConnection.Open();

        const string selectSql = """
            SELECT CustomerId, Name, Document, Email, Status
            FROM Customers
            ORDER BY CustomerId;
            """;

        const string insertSql = """
            INSERT INTO Customers
                (CustomerId, Name, Document, Email, Status)
            VALUES
                (@CustomerId, @Name, @Document, @Email, @Status);
            """;

        using var selectCommand = new SqlCommand(
            selectSql,
            sourceConnection);

        using var reader = selectCommand.ExecuteReader();

        while (reader.Read())
        {
            using var insertCommand = new SqlCommand(
                insertSql,
                targetConnection);

            insertCommand.Parameters.AddWithValue(
                "@CustomerId",
                reader["CustomerId"]);

            insertCommand.Parameters.AddWithValue(
                "@Name",
                reader["Name"]);

            insertCommand.Parameters.AddWithValue(
                "@Document",
                reader["Document"]);

            insertCommand.Parameters.AddWithValue(
                "@Email",
                reader["Email"]);

            insertCommand.Parameters.AddWithValue(
                "@Status",
                reader["Status"]);

            insertCommand.ExecuteNonQuery();
        }
    }
}

