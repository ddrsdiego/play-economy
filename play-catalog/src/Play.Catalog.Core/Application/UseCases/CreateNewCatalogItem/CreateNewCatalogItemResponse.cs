namespace Play.Catalog.Core.Application.UseCases.CreateNewCatalogItem
{
    public record CreateNewCatalogItemResponse(string Id, string Name, string Description, decimal UnitPrice,
        DateTimeOffset CreatedAt);
}