namespace Play.Inventory.Core.Application.Infra.Repositories.CatalogItemRepository
{
    using Domain.AggregateModel.CatalogItemAggregate;

    public static class CatalogItemDataEx
    {
        public static CatalogItemStateEntry ToCatalogItem(this CatalogItem catalogItem)
        {
            return new CatalogItemStateEntry
            {
                CatalogItemId = catalogItem.CatalogItemId,
                Description = catalogItem.Description,
                Name = catalogItem.Name,
                CreatedAt = catalogItem.CreatedAt
            };
        }

        public static CatalogItem ToStateEntry(this CatalogItemStateEntry catalogItemStateEntry)
        {
            var catalogItem = new CatalogItem(catalogItemStateEntry.CatalogItemId, catalogItemStateEntry.Name,
                catalogItemStateEntry.Description, catalogItemStateEntry.CreatedAt);

            return catalogItem;
        }
    }
}