namespace Play.Inventory.Core.Application.Responses
{
    public sealed record GetInventoryItemByUserIdResponse(string Name, string Email,
        IEnumerable<GetInventoryItemLineResponse> Items);

    public sealed record GetInventoryItemLineResponse(string CatalogItemId, string Name, string Description,
        int Quantity, DateTimeOffset AcquiredAt);
}