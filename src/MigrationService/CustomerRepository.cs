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
}