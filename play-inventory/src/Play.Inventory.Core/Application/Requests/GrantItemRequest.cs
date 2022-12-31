namespace Play.Inventory.Core.Application.Requests
{
    public record GrantItemRequest(string UserId, string CatalogItemId, int Quantity);

    public record InventoryItemResponse(string CatalogItemId, int Quantity, DateTimeOffset AcquiredAt);
}