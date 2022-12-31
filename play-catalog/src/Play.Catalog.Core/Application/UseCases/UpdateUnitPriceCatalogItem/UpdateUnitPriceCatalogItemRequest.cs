namespace Play.Catalog.Core.Application.UseCases.UpdateUnitPriceCatalogItem
{
    public record UpdateUnitPriceCatalogItemRequest(string CatalogItemId, decimal UnitPrice);
}