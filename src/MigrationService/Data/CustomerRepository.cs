using Microsoft.Data.SqlClient;

namespace MigrationService.Data;

public class CustomerRepository
{
    private readonly string _connectionString;

    public CustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Customer> GetCustomers()
    {
        var customers = new List<Customer>();

        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        const string sql = """
            SELECT CustomerId, Name, Document, Email, Address, Status
            FROM Customers
            ORDER BY CustomerId;
            """;

        using var command = new SqlCommand(sql, connection);
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
                Status = reader.GetString(reader.GetOrdinal("Status"))
            });
        }

        return customers;
    }
}