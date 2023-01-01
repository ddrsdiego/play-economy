namespace Play.Catalog.Core.Application.Infra.Repositories
{
    using Common.Application.Infra.Repositories.Dapr;
    using Dapr.Client;
    using Microsoft.Extensions.Logging;
    using Domain.AggregatesModel.CatalogItemAggregate;

    public sealed class CatalogItemRepository : ICatalogItemRepository
    {
        public const string StateStoreName = "play-catalog-state-store";

        private readonly DaprClient _daprClient;
        private readonly ILogger<CatalogItemRepository> _logger;

        public CatalogItemRepository(ILogger<CatalogItemRepository> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        public Task SaveOrUpdateAsync(CatalogItem catalogItem)
        {
            var stateStoreKey = KeyFormatterHelper.ConstructStateStoreKey(nameof(CatalogItem), catalogItem.Id);

            try
            {
                var data = catalogItem.ToCatalogItemData();

                var slowSaveStateAsync = _daprClient.SaveStateAsync(StateStoreName, stateStoreKey,
                    data);

                return slowSaveStateAsync.IsCompletedSuccessfully ? Task.CompletedTask : SlowTask(slowSaveStateAsync);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<CatalogItem> GetByIdAsync(string? id)
        {
            var stateStoreKey = KeyFormatterHelper.ConstructStateStoreKey(nameof(CatalogItem), id);

            try
            {
                var data = await _daprClient.GetStateEntryAsync<CatalogItemData>(StateStoreName, stateStoreKey);

                return (data.Value is null ? CatalogItem.Default : data.Value.ToCatalogItem()) ?? throw new InvalidOperationException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async Task SlowTask(Task task) => await task;
    }

    public class CatalogItemData : IDaprStateEntry
    {
        public string? Id { get; set; }
        public string? CatalogItemId { get; set; }
        public string CatalogItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreateAt { get; set; }
    }
}