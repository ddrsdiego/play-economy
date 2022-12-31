namespace Play.Customer.Core.Application.Infra.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapr.Client;
    using Domain.AggregateModel.CustomerAggregate;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;

    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly AppSettings _appSettings;
        private readonly DaprClient _daprClient;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(ILogger<CustomerRepository> logger, DaprClient daprClient,
            IOptions<AppSettings> options)
        {
            _logger = logger;
            _daprClient = daprClient;
            _appSettings = options.Value;
        }

        public Task<Customer> GetById(string customerId) => GetByFormattedKey(customerId);

        public Task<Customer> GetByEmailAsync(string email) => GetByFormattedKey(email);

        public Task<Customer> GetByDocumentAsync(string document) => GetByFormattedKey(document);

        private async Task<Customer> GetByFormattedKey(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var formattedKey = CustomerData.GetKeyFormatted(key);

            try
            {
                var state = await _daprClient.GetStateEntryAsync<CustomerData>(_appSettings.DaprSettings.StateStoreName,
                    formattedKey);

                return state.Value == null ? Customer.Default : state.Value.ToCustomer();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
                throw;
            }
        }

        public Task UpsertAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            var key = customer.Identification.Value;
            var emailKey = CustomerData.GetKeyFormatted(customer.Identification.Email);
            var customerIdKey = CustomerData.GetKeyFormatted(customer.Identification.Id);
            var documentKey = CustomerData.GetKeyFormatted(customer.Identification.Document);

            try
            {
                var data = customer.ToCustomerData();

                var requests = new List<StateTransactionRequest>(4)
                {
                    new(key, JsonSerializer.SerializeToUtf8Bytes(data), StateOperationType.Upsert),
                    new(emailKey, JsonSerializer.SerializeToUtf8Bytes(data), StateOperationType.Upsert),
                    new(customerIdKey, JsonSerializer.SerializeToUtf8Bytes(data), StateOperationType.Upsert),
                    new(documentKey, JsonSerializer.SerializeToUtf8Bytes(data), StateOperationType.Upsert)
                };

                var task = _daprClient.ExecuteStateTransactionAsync(_appSettings.DaprSettings.StateStoreName, requests,
                    cancellationToken: cancellationToken);

                return task.IsCompletedSuccessfully ? Task.CompletedTask : SlowExecute(task);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
                throw;
            }

            static async Task SlowExecute(Task task) => await task;
        }
    }

    internal sealed class CustomerData
    {
        public string Id { get; set; }
        public CustomerPropertiesData Properties { get; set; }

        public static string GetKeyFormatted(string value) => $"{nameof(Customer).ToLowerInvariant()}-{value}";
    }

    internal sealed class CustomerPropertiesData
    {
        public string CustomerId { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}