namespace Play.Inventory.Core.Application.Infra.Repositories
{
    using Clients;
    using Dapr.Client;
    using Domain.AggregateModel.CustomerAggregate;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public sealed class CustomerRepository : Repository, ICustomerRepository
    {
        private readonly DaprClient _daprClient;
        private readonly ICustomerClient _customerClient;

        public CustomerRepository(ILoggerFactory logger, ICustomerClient customerClient, DaprClient daprClient,
            IOptions<AppSettings> options)
            : base(logger.CreateLogger<CustomerRepository>(), daprClient, options.Value)
        {
            _customerClient = customerClient;
            _daprClient = daprClient;
        }

        public async Task<Customer> GetCustomerByIdAsync(string userId)
        {
            var key = CustomerData.GetKeyFormatted(userId);
            var state = await _daprClient.GetStateEntryAsync<CustomerData>(AppSettings.DaprSettings.StateStoreName,
                key);

            if (state.Value != null) 
                return state.Value.ToCustomer();

            var customerClientResult = await _customerClient.GetCustomerById(userId);
            if (customerClientResult.IsFailure)
                return Customer.Default;

            var customerEntity = new Customer(customerClientResult.Value.CustomerId, customerClientResult.Value.Name,
                customerClientResult.Value.Email, DateTimeOffset.UtcNow);

            await UpsertAsync(customerEntity);
            return customerEntity;
        }

        public async Task UpsertAsync(Customer customer)
        {
            var key = CustomerData.GetKeyFormatted(customer.CustomerId);

            try
            {
                var data = customer.ToCustomerData();
                await _daprClient.SaveStateAsync(AppSettings.DaprSettings.StateStoreName, key, data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    internal class CustomerData
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        
        public static string GetKeyFormatted(string value) => $"{nameof(Customer).ToLowerInvariant()}-{value}";
    }

    internal static class CustomerDataEx
    {
        public static CustomerData ToCustomerData(this Customer customer)
        {
            return new CustomerData
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                CreatedAt = customer.CreatedAt
            };
        }
        
        public static Customer ToCustomer(this CustomerData data)
        {
            return new Customer(data.CustomerId, data.Name, data.Email, data.CreatedAt);
        }
    }
}