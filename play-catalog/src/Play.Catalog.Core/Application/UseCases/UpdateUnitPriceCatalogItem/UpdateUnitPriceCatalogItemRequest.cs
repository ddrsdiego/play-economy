namespace Play.Catalog.Core.Application.UseCases.UpdateUnitPriceCatalogItem
{
    using Common.Application.UseCase;

    public class UpdateUnitPriceCatalogItemRequest : UseCaseRequest
    {
        public UpdateUnitPriceCatalogItemRequest(string catalogItemId, decimal unitPrice)
        {
            UnitPrice = unitPrice;
            CatalogItemId = catalogItemId;
        }

        public string CatalogItemId { get; }
        public decimal UnitPrice { get; }
    }
}