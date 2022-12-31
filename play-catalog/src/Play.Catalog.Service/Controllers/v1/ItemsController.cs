namespace Play.Catalog.Service.Controllers.v1
{
    using System.Threading.Tasks;
    using Core;
    using Core.Application.Helpers;
    using Core.Application.Requests;
    using Core.Application.Responses;
    using Core.Application.UseCases.CreateNewCatalogItem;
    using Core.Application.UseCases.UpdateUnitPriceCatalogItem;
    using Core.Domain.AggregatesModel.CatalogItemAggregate;
    using Dapr.Client;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly DaprClient _daprClient;

        private readonly IUseCaseExecutor<CreateNewCatalogItemRequest, CreateNewCatalogItemResponse>
            _createNewCatalogItemUseCase;

        private readonly IUseCaseExecutor<UpdateUnitPriceCatalogItemRequest, UpdateUnitPriceCatalogItemResponse>
            _updateUnitPriceCatalogUseCase;

        public ItemsController(ICatalogItemRepository catalogItemRepository, DaprClient daprClient,
            IUseCaseExecutor<CreateNewCatalogItemRequest, CreateNewCatalogItemResponse> createNewCatalogItemUseCase,
            IUseCaseExecutor<UpdateUnitPriceCatalogItemRequest, UpdateUnitPriceCatalogItemResponse>
                updateUnitPriceCatalogUseCase)
        {
            _catalogItemRepository = catalogItemRepository;
            _daprClient = daprClient;
            _createNewCatalogItemUseCase = createNewCatalogItemUseCase;
            _updateUnitPriceCatalogUseCase = updateUnitPriceCatalogUseCase;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var catalogItem = await _catalogItemRepository.GetByIdAsync(id);

            if (catalogItem.Equals(CatalogItem.Default))
                return NoContent();

            var itemResponse = new ItemResponse(catalogItem.Id, catalogItem.Description.Name,
                catalogItem.Description.Value, catalogItem.Price.Value, catalogItem.CreateAt);

            return Ok(itemResponse);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateItemRequest request)
        {
            var response =
                await _createNewCatalogItemUseCase.SendAsync(
                    new CreateNewCatalogItemRequest(request.Price, request.Name, request.Description));

            return CreatedAtAction(nameof(GetById), new {id = response.Id}, response);
        }

        [HttpPut("{id}/unit-price")]
        public async Task<IActionResult> PutAsync(string id, decimal unitPrice)
        {
            var response =
                await _updateUnitPriceCatalogUseCase.SendAsync(new UpdateUnitPriceCatalogItemRequest(id, unitPrice));

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnitPrice(string id, UpdateUnitPriceCatalogItemRequest request)
        {
            var catalogItem = await _catalogItemRepository.GetByIdAsync(id);

            var response =
                await _updateUnitPriceCatalogUseCase.SendAsync(request);

            // catalogItem.UpdateUnitePrice(request.UnitPrice);
            // catalogItem.UpdateDescription(new CatalogItemDescription(request.Name, request.Description));
            //
            // await _catalogItemRepository.SaveOrUpdateAsync(catalogItem);
            //
            // var catalogItemUpdated = new CatalogItemUpdated(catalogItem.Id, catalogItem.Description.Name,
            //     catalogItem.Description.Value);
            //
            // await _daprClient.PublishEventAsync("play-catalog-service", Topics.CatalogItemUpdated, catalogItemUpdated);

            return NoContent();
        }
    }
}