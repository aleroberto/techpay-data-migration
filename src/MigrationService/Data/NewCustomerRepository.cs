using Microsoft.Data.SqlClient;

namespace MigrationService.Data;

public class NewCustomerRepository
{
    private readonly string _connectionString;

    public NewCustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DateTime? GetLastSync(string entityName)
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        const string sql = """
            SELECT LastSyncAt
            FROM MigrationControl
            WHERE EntityName = @EntityName;
            """;

        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@EntityName", entityName);

        var result = command.ExecuteScalar();

        return result == null || result == DBNull.Value
            ? null
            : (DateTime)result;
    }

    public void Upsert(Customer customer)
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        const string sql = """
            UPDATE Customers
            SET
                Name = @Name,
                Document = @Document,
                Email = @Email,
                Status = @Status
            WHERE CustomerId = @CustomerId;

            IF @@ROWCOUNT = 0
            BEGIN
                INSERT INTO Customers
                    (CustomerId, Name, Document, Email, Status, CreatedAt)
                VALUES
                    (@CustomerId, @Name, @Document, @Email, @Status, @CreatedAt);
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

    public void UpdateLastSync(string entityName, DateTime lastSync)
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        const string sql = """
            UPDATE MigrationControl
            SET LastSyncAt = @LastSyncAt
            WHERE EntityName = @EntityName;

            IF @@ROWCOUNT = 0
            BEGIN
                INSERT INTO MigrationControl
                    (EntityName, LastSyncAt)
                VALUES
                    (@EntityName, @LastSyncAt);
            END
            """;

        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@EntityName", entityName);
        command.Parameters.AddWithValue("@LastSyncAt", lastSync);

        command.ExecuteNonQuery();
    }
}