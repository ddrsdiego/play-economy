namespace Play.Catalog.Core.Application.Requests
{
    public readonly struct GetItemRequest
    {
        public GetItemRequest(string id) => Id = id;

        public string Id { get; }
    }
}