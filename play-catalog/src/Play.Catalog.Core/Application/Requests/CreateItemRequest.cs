namespace Play.Catalog.Core.Application.Requests
{
    public class CreateItemRequest
    {
        public CreateItemRequest(string? name, string? description, decimal price, DateTimeOffset createdAt)
        {
            Name = name;
            Description = description;
            Price = price;
            CreatedAt = createdAt;
        }
        
        public string? Name { get; }
        public string? Description { get; }
        public decimal Price { get; }
        public DateTimeOffset CreatedAt { get; }
    }
}