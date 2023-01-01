namespace Play.Catalog.Core.Application.UseCases.CreateNewCatalogItem
{
    using Common.Application.UseCase;

    public class CreateNewCatalogItemRequest : UseCaseRequest
    {

        public CreateNewCatalogItemRequest(decimal price, string name, string description)
        {
            Price = price;
            Name = name;
            Description = description;
        }
        
        public decimal Price { get; }
        public string Name { get; }
        public string Description { get; }
    }
}