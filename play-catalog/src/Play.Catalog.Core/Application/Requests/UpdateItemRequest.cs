namespace Play.Catalog.Core.Application.Requests
{
    public class UpdateItemRequest
    {
        public UpdateItemRequest(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
        
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }
    }
}