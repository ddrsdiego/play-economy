namespace Play.Catalog.Core.Application.Infra.Repositories
{
    using Common.Application.Infra.Repositories.Dapr;
    using Dapr.Client;

    [StateEntryName("catalog-item")]
    public class CatalogItemData : IDaprStateEntry
    {
        public const string StateStoreName = "play-catalog-state-store";
        
        public string? Id { get; set; }
        public string? CatalogItemId { get; set; }
        public string CatalogItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreateAt { get; set; }
    }
    
    public sealed class CatalogItemDaprRepository : DaprStateEntryRepository<CatalogItemData>
    {
        public CatalogItemDaprRepository(DaprClient daprClient)
            : base(CatalogItemData.StateStoreName, daprClient)
        {
        }
    }
}