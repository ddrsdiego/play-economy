namespace Play.Inventory.Core.Application.Infra.Repositories
{
    using Dapr.Client;
    using Domain.AggregateModel.CustomerAggregate;
    using Domain.AggregateModel.InventoryItemAggregate;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public sealed class InventoryItemRepository : Repository, IInventoryItemRepository
    {
        public InventoryItemRepository(ILoggerFactory logger, DaprClient daprClient, IOptions<AppSettings> options)
            : base(logger.CreateLogger<InventoryItemRepository>(), daprClient, options.Value)
        {
        }

        public async Task SaveOrUpdateAsync(InventoryItem inventoryItem)
        {
            var key = $"{nameof(InventoryItem).ToLowerInvariant()}-{inventoryItem.Customer.CustomerId}";

            try
            {
                var data = inventoryItem.ToInventoryItemData();
                await DaprClient.SaveStateAsync(AppSettings.DaprSettings.StateStoreName, key, data);
            }
            catch (Exception e)
            {
                throw;
            }

            static async Task SlowTask(Task task) => await task;
        }

        public async Task<InventoryItem> GetByUserIdAsync(string userId)
        {
            try
            {
                var customerKey = CustomerData.GetKeyFormatted(userId);
                var inventoryKey = $"{nameof(InventoryItem).ToLowerInvariant()}-{userId}";

                var getCustomerTask = DaprClient.GetStateEntryAsync<CustomerData>(AppSettings.DaprSettings.StateStoreName,
                        customerKey);
                var getInventoryItemTask = DaprClient.GetStateEntryAsync<InventoryItemData>(AppSettings.DaprSettings.StateStoreName,
                    inventoryKey);

                await Task.WhenAll(getCustomerTask, getInventoryItemTask);

                var customerData = await getCustomerTask;
                var inventoryItemData = await getInventoryItemTask;
                
                return inventoryItemData.Value == null
                    ? InventoryItem.Default
                    : inventoryItemData.Value.ToInventoryItem(customerData.Value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    internal class InventoryItemData
    {
        public string UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public IEnumerable<InventoryItemLineData> Items { get; init; }
    }

    internal class InventoryItemLineData
    {
        public string CatalogItemId { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset AcquiredAt { get; set; }
    }
}