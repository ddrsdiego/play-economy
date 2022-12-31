namespace Play.Catalog.Core.Application.UseCases.CreateNewCatalogItem
{
    using Dapr.Client;
    using Domain.AggregatesModel.CatalogItemAggregate;
    using Helpers;
    using Microsoft.Extensions.Logging;

    public sealed class
        CreateNewCatalogItemUseCase : UseCaseExecutor<CreateNewCatalogItemRequest, CreateNewCatalogItemResponse>
    {
        private readonly DaprClient _daprClient;
        private readonly ICatalogItemRepository _catalogItemRepository;

        public CreateNewCatalogItemUseCase(ILoggerFactory logger, ICatalogItemRepository catalogItemRepository,
            DaprClient daprClient)
            : base(logger.CreateLogger<CreateNewCatalogItemUseCase>())
        {
            _catalogItemRepository = catalogItemRepository;
            _daprClient = daprClient;
        }

        protected override async Task<CreateNewCatalogItemResponse> ActionAsync(CreateNewCatalogItemRequest request,
            CancellationToken token = default)
        {
            var newCatalogItem = new CatalogItem(request.Price, request.Name, request.Description);

            await _catalogItemRepository.SaveOrUpdateAsync(newCatalogItem);

            var catalogItemCreated = new CatalogItemCreated(newCatalogItem.Id, newCatalogItem.Description.Name,
                newCatalogItem.Description.Value);

            await _daprClient.PublishEventAsync("play-catalog-service", Topics.CatalogItemCreated, catalogItemCreated,
                token);

            return new CreateNewCatalogItemResponse(
                newCatalogItem.Id,
                newCatalogItem.Description.Name,
                newCatalogItem.Description.Value,
                newCatalogItem.Price.Value,
                newCatalogItem.CreateAt
                );
        }
    }
}