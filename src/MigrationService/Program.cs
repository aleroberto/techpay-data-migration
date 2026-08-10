using Microsoft.Data.SqlClient;

var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");

if (string.IsNullOrWhiteSpace(password))
{
    Console.WriteLine("Variável MSSQL_SA_PASSWORD não configurada.");
    return;
}

var connectionString =
    $"Server=host.docker.internal,1433;" +
    $"Database=master;" +
    $"User Id=sa;" +
    $"Password={password};" +
    $"TrustServerCertificate=True;";

try
{
    await using var connection = new SqlConnection(connectionString);

    await connection.OpenAsync();

    Console.WriteLine("Conexão com SQL Server realizada com sucesso.");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao conectar ao SQL Server: {ex.Message}");
}