namespace Play.Inventory.Core.Application.UseCases.CreateCatalogItem
{
    using Common.Application.UseCase;

    public sealed class CreateCatalogItemReq : UseCaseRequest
    {
        public CreateCatalogItemReq(string catalogItemId, string name, string description)
        {
            CatalogItemId = catalogItemId;
            Name = name;
            Description = description;
        }

        public string CatalogItemId { get; }
        public string Name { get; }
        public string Description { get; }
    }
}