namespace MigrationService.Data;

public class CustomerMigrationService
{
    private readonly CustomerRepository _legacyRepository;
    private readonly NewCustomerRepository _newRepository;

    public CustomerMigrationService(
        CustomerRepository legacyRepository,
        NewCustomerRepository newRepository)
    {
        _legacyRepository = legacyRepository;
        _newRepository = newRepository;
    }

    public void Migrate()
    {
        var lastSync = _newRepository.GetLastSync("Customers");

        var customers = _legacyRepository.GetCustomers(lastSync);

        Console.WriteLine($"Clientes encontrados para sincronização: {customers.Count}");

        foreach (var customer in customers)
        {
            _newRepository.Upsert(customer);

            Console.WriteLine(
                $"Cliente processado: {customer.CustomerId} - {customer.Name}");
        }

        if (customers.Count > 0)
        {
            var lastProcessed = customers.Max(c => c.UpdatedAt);

            _newRepository.UpdateLastSync(
                "Customers",
                lastProcessed);
        }

        Console.WriteLine("Sincronização concluída.");
    }
}