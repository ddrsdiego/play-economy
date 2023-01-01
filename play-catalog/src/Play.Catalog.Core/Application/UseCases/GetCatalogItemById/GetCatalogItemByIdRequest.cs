namespace Play.Catalog.Core.Application.UseCases.GetCatalogItemById
{
    using Common.Application.UseCase;

    public sealed class GetCatalogItemByIdRequest : UseCaseRequest
    {
        public GetCatalogItemByIdRequest(string id) => Id = id;

        public string Id { get; }
    }
}