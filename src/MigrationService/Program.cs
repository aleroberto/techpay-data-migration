using MigrationService.Data;

var dbPassword = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");

if (string.IsNullOrWhiteSpace(dbPassword))
{
    Console.WriteLine("Erro de senha ao conectar.");
    return;
}

string legacyConnectionString =
    $"Server=techpay-sqlserver,1433;" +
    $"Database=LegacyDb;" +
    $"User Id=sa;" +
    $"Password={dbPassword};" +
    $"TrustServerCertificate=True;";

string newConnectionString =
    $"Server=techpay-sqlserver,1433;" +
    $"Database=NewDb;" +
    $"User Id=sa;" +
    $"Password={dbPassword};" +
    $"TrustServerCertificate=True;";

var legacyRepository = new CustomerRepository(legacyConnectionString);
var newRepository = new NewCustomerRepository(newConnectionString);

var customers = legacyRepository.GetCustomers();

Console.WriteLine($"Clientes encontrados no legado: {customers.Count}");

foreach (var customer in customers)
{
    var newCustomer = new NewCustomer
    {
        CustomerId = customer.CustomerId,
        Name = customer.Name,
        Document = customer.Document,
        Email = customer.Email,
        Status = customer.Status
    };

    newRepository.InsertIfNotExists(newCustomer);

    Console.WriteLine(
        $"Cliente processado: {newCustomer.CustomerId} - {newCustomer.Name}");
}

Console.WriteLine("Migração concluída.");