namespace Play.Inventory.Core.Application.Infra.Repositories
{
    using System.Text.Json;
    using Dapr.Client;
    using Domain.AggregateModel.CatalogItemAggregate;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public sealed class CatalogItemRepository : Repository, ICatalogItemRepository
    {
        public CatalogItemRepository(ILoggerFactory logger, DaprClient daprClient, IOptions<AppSettings> options)
            : base(logger.CreateLogger<CatalogItemRepository>(), daprClient, options.Value)
        {
        }

        public async Task<CatalogItem> GetByIdAsync(string catalogItemId)
        {
            var key = $"{nameof(CatalogItem).ToLowerInvariant()}-{catalogItemId}";

            var state = await DaprClient.GetStateEntryAsync<CatalogItemData>(AppSettings.DaprSettings.StateStoreName,
                key);

            return state.Value is null ? CatalogItem.Default : state.Value.ToCatalogItem();
        }

        public async Task<IReadOnlyCollection<CatalogItem>> GetByIdsAsync(string[] catalogItemIds)
        {
            var keys = catalogItemIds.Select(catalogItemId =>
                $"{nameof(CatalogItem).ToLowerInvariant()}-{catalogItemId}").ToList();


            var results = await DaprClient.GetBulkStateAsync(
                AppSettings.DaprSettings.StateStoreName, keys, parallelism: 1);

            var catalogItems = results.Select(x =>
            {
                var catalogItemData = JsonSerializer.Deserialize<CatalogItemData>(x.Value, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return new CatalogItem(catalogItemData.CatalogItemId, catalogItemData.Name, catalogItemData.Description,
                    catalogItemData.CreatedAt);
            }).ToList().AsReadOnly();

            return catalogItems;
        }

        public async Task UpsertAsync(CatalogItem newCatalogItem)
        {
            var key = $"{nameof(CatalogItem).ToLowerInvariant()}-{newCatalogItem.CatalogItemId}";
            var data = newCatalogItem.ToCatalogItemData();

            await DaprClient.SaveStateAsync(AppSettings.DaprSettings.StateStoreName, key, data);
        }
    }

    internal class CatalogItemData
    {
        public string CatalogItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    internal static class CatalogItemDataEx
    {
        public static CatalogItemData ToCatalogItemData(this CatalogItem catalogItem)
        {
            return new CatalogItemData
            {
                CatalogItemId = catalogItem.CatalogItemId,
                Description = catalogItem.Description,
                Name = catalogItem.Name,
                CreatedAt = catalogItem.CreatedAt
            };
        }

        public static CatalogItem ToCatalogItem(this CatalogItemData catalogItemData)
        {
            var catalogItem = new CatalogItem(catalogItemData.CatalogItemId, catalogItemData.Name,
                catalogItemData.Description, catalogItemData.CreatedAt);

            return catalogItem;
        }
    }
}