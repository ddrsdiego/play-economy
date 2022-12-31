namespace Play.Catalog.Core.Application.UseCases.UpdateUnitPriceCatalogItem
{
    using Dapr.Client;
    using Domain.AggregatesModel.CatalogItemAggregate;
    using Microsoft.Extensions.Logging;
    using Helpers;

    public sealed class
        UpdateUnitPriceCatalogItemUseCase : UseCaseExecutor<UpdateUnitPriceCatalogItemRequest,
            UpdateUnitPriceCatalogItemResponse>
    {
        private readonly DaprClient _daprClient;
        private readonly ICatalogItemRepository _catalogItemRepository;

        public UpdateUnitPriceCatalogItemUseCase(ILoggerFactory logger, ICatalogItemRepository catalogItemRepository,
            DaprClient daprClient)
            : base(logger.CreateLogger<UpdateUnitPriceCatalogItemUseCase>())
        {
            _catalogItemRepository = catalogItemRepository;
            _daprClient = daprClient;
        }

        protected override async Task<UpdateUnitPriceCatalogItemResponse> ActionAsync(
            UpdateUnitPriceCatalogItemRequest request, CancellationToken token = default)
        {
            var catalogItem = await _catalogItemRepository.GetByIdAsync(request.CatalogItemId);

            var newUnitPrice = new UnitPrice(request.UnitPrice);

            catalogItem.UpdateUnitePrice(newUnitPrice);

            await _catalogItemRepository.SaveOrUpdateAsync(catalogItem);

            var catalogItemUpdated = new CatalogItemUpdated(catalogItem.Id, catalogItem.Description.Name,
                catalogItem.Description.Value);

            await _daprClient.PublishEventAsync("play-catalog-service", Topics.CatalogItemUpdated, catalogItemUpdated, token);

            return new UpdateUnitPriceCatalogItemResponse();
        }
    }
}