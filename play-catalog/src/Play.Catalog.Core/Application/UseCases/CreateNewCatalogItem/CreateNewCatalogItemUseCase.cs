namespace Play.Catalog.Core.Application.UseCases.CreateNewCatalogItem
{
    using Common.Application;
    using Common.Application.Infra.Repositories.Dapr;
    using Common.Application.UseCase;
    using Dapr.Client;
    using Domain.AggregatesModel.CatalogItemAggregate;
    using Helpers;
    using Infra.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public sealed class
        CreateNewCatalogItemUseCase : UseCaseExecutor<CreateNewCatalogItemRequest>
    {
        private readonly DaprClient _daprClient;
        private readonly IDaprStateEntryRepository<CatalogItemData> _daprStateEntryRepository;

        public CreateNewCatalogItemUseCase(ILoggerFactory logger, DaprClient daprClient,
            IDaprStateEntryRepository<CatalogItemData> daprStateEntryRepository)
            : base(logger.CreateLogger<CreateNewCatalogItemUseCase>())
        {
            _daprClient = daprClient;
            _daprStateEntryRepository = daprStateEntryRepository;
        }

        protected override async Task<Response> ExecuteSendAsync(CreateNewCatalogItemRequest request,
            CancellationToken token = default)
        {
            var newCatalogItem = new CatalogItem(request.Price, request.Name, request.Description);

            await _daprStateEntryRepository.UpsertAsync(newCatalogItem.ToCatalogItemData(), token);

            var catalogItemCreated = new CatalogItemCreated(newCatalogItem.Id, newCatalogItem.Description.Name,
                newCatalogItem.Description.Value);

            await _daprClient.PublishEventAsync("play-catalog-service", Topics.CatalogItemCreated, catalogItemCreated,
                token);

            var responseContent = new CreateNewCatalogItemResponse(
                newCatalogItem.Id,
                newCatalogItem.Description.Name,
                newCatalogItem.Description.Value,
                newCatalogItem.Price.Value,
                newCatalogItem.CreateAt
            );

            return Response.Ok(ResponseContent.Create(responseContent), StatusCodes.Status201Created);
        }
    }
}