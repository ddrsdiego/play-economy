namespace Play.Catalog.Core.Application.UseCases.GetCatalogItemById
{
    using Common.Application;
    using Common.Application.Infra.Repositories.Dapr;
    using Common.Application.UseCase;
    using Infra.Repositories;
    using Microsoft.Extensions.Logging;

    public sealed class GetCatalogItemByIdUseCase : UseCaseExecutor<GetCatalogItemByIdRequest>
    {
        private readonly IDaprStateEntryRepository<CatalogItemData> _entryRepository;

        public GetCatalogItemByIdUseCase(ILoggerFactory logger,
            IDaprStateEntryRepository<CatalogItemData> entryRepository)
            : base(logger.CreateLogger<GetCatalogItemByIdUseCase>())
        {
            _entryRepository = entryRepository;
        }

        protected override async Task<Response> ExecuteSendAsync(GetCatalogItemByIdRequest request,
            CancellationToken token = default)
        {
            var catalogItemData = await _entryRepository.GetByIdAsync(request.Id, token);
            if (catalogItemData.IsFailure)
                return Response.Fail(new Error("", ""));

            var response = new GetCatalogItemByIdResponse(
                catalogItemData.Value.CatalogItemId,
                catalogItemData.Value.CatalogItemName,
                catalogItemData.Value.Description,
                catalogItemData.Value.Price,
                catalogItemData.Value.CreateAt
            );
            
            return Response.Ok(ResponseContent.Create(response));
        }
    }
}