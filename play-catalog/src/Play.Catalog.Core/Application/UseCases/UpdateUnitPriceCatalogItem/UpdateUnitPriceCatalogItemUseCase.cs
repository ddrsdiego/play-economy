namespace Play.Catalog.Core.Application.UseCases.UpdateUnitPriceCatalogItem
{
    using Common.Application;
    using Common.Application.Infra.Repositories.Dapr;
    using Common.Application.UseCase;
    using Dapr.Client;
    using Domain.AggregatesModel.CatalogItemAggregate;
    using Microsoft.Extensions.Logging;
    using Helpers;
    using Infra.Repositories;
    using Microsoft.AspNetCore.Http;

    public sealed class UpdateUnitPriceCatalogItemUseCase : UseCaseExecutor<UpdateUnitPriceCatalogItemRequest>
    {
        private readonly DaprClient _daprClient;
        private readonly IDaprStateEntryRepository<CatalogItemData> _daprStateEntryRepository;
        private const string ItemNotFoundError = "ITEM_NOT_FOUND";
        
        public UpdateUnitPriceCatalogItemUseCase(ILoggerFactory logger, DaprClient daprClient,
            IDaprStateEntryRepository<CatalogItemData> daprStateEntryRepository)
            : base(logger.CreateLogger<UpdateUnitPriceCatalogItemUseCase>())
        {
            _daprClient = daprClient;
            _daprStateEntryRepository = daprStateEntryRepository;
        }

        protected override async Task<Response> ExecuteSendAsync(UpdateUnitPriceCatalogItemRequest request,
            CancellationToken token = default)
        {
            var stateEntry = await _daprStateEntryRepository.GetByIdAsync(request.CatalogItemId, token);
            if (stateEntry.IsFailure)
                return Response.Fail(new Error(ItemNotFoundError, $"Item not found in catalog with id {request.CatalogItemId}"));

            var catalogItem = stateEntry.Value.ToCatalogItem();
            var newUnitPrice = new UnitPrice(request.UnitPrice);

            catalogItem.UpdateUnitePrice(newUnitPrice);

            var data = catalogItem.ToCatalogItemData();
            await _daprStateEntryRepository.UpsertAsync(data, token);

            var catalogItemUpdated = new CatalogItemUpdated(catalogItem.Id, catalogItem.Description.Name,
                catalogItem.Description.Value,
                catalogItem.Price.Value);

            await _daprClient.PublishEventAsync("play-catalog-service", Topics.CatalogItemUpdated, catalogItemUpdated,
                token);

            return Response.Ok(StatusCodes.Status200OK);
        }
    }
}