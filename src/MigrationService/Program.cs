using MigrationService.Data;

var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");

if (string.IsNullOrWhiteSpace(password))
{
    Console.WriteLine("Eroo DB_PASSWORD.");
    return;
}

string connectionString =
    $"Server=techpay-sqlserver,1433;Database=LegacyDb;User Id=sa;Password={password};TrustServerCertificate=True;";

var database = new LegacyDbConnection(connectionString);

using var connection = database.CreateConnection();

try
{
    connection.Open();

    Console.WriteLine("Conexão com SQL Server realizada com sucesso.");

    var repository = new CustomerRepository(connectionString);

    repository.ListCustomers();
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao conectar ao SQL Server: {ex.Message}");
}