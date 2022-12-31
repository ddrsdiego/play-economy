namespace Play.Catalog.Core.Application.Responses
{
    public record ItemResponse(string Id, string Name, string Description, decimal Price, DateTimeOffset CreatedAt);
}