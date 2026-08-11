using Microsoft.Data.SqlClient;

namespace MigrationService.Data;

public class NewCustomerRepository
{
    private readonly string _connectionString;

    public NewCustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InsertIfNotExists(NewCustomer customer)
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        const string sql = """
            IF NOT EXISTS (
                SELECT 1
                FROM Customers
                WHERE CustomerId = @CustomerId
            )
            BEGIN
                INSERT INTO Customers
                    (CustomerId, Name, Document, Email, Status, CreatedAt)
                VALUES
                    (@CustomerId, @Name, @Document, @Email, @Status, @CreatedAt)
            END
            """;

        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
        command.Parameters.AddWithValue("@Name", customer.Name);
        command.Parameters.AddWithValue("@Document", customer.Document);
        command.Parameters.AddWithValue("@Email", customer.Email);
        command.Parameters.AddWithValue("@Status", customer.Status);
        command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

        command.ExecuteNonQuery();
    }
}