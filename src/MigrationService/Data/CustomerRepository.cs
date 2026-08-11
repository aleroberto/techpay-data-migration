using Microsoft.Data.SqlClient;

namespace MigrationService.Data;

public class CustomerRepository
{
    private readonly string _connectionString;

    public CustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Customer> GetCustomers(DateTime? lastSync)
    {
        var customers = new List<Customer>();

        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        const string sql = """
            SELECT CustomerId, Name, Document, Email, Address, Status, UpdatedAt
            FROM Customers
            WHERE @LastSync IS NULL OR UpdatedAt > @LastSync
            ORDER BY UpdatedAt, CustomerId;
            """;

        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "@LastSync",
            (object?)lastSync ?? DBNull.Value);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            customers.Add(new Customer
            {
                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Document = reader.GetString(reader.GetOrdinal("Document")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Address")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            });
        }

        return customers;
    }
}