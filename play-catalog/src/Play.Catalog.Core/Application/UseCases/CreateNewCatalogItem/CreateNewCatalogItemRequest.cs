namespace Play.Catalog.Core.Application.UseCases.CreateNewCatalogItem
{
    public record CreateNewCatalogItemRequest(decimal Price, string Name, string Description);
}