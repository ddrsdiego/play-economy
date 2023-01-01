namespace Play.Inventory.Core.Application.Infra.Repositories.InventoryItemRepository
{
    using Play.Common.Application.Infra.Repositories.Dapr;

    public class InventoryItemStateEntry : IDaprStateEntry
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public IEnumerable<InventoryItemLineStateEntry> Items { get; init; }
    }
    
    public class InventoryItemLineStateEntry
    {
        public string CatalogItemId { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset AcquiredAt { get; set; }
    }
}