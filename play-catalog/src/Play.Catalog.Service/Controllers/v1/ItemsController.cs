namespace Play.Catalog.Service.Controllers.v1
{
    using System.Threading.Tasks;
    using Common.Application.UseCase;
    using Core.Application.Requests;
    using Core.Application.UseCases.CreateNewCatalogItem;
    using Core.Application.UseCases.GetCatalogItemById;
    using Core.Application.UseCases.UpdateUnitPriceCatalogItem;
    using Core.Domain.AggregatesModel.CatalogItemAggregate;
    using Dapr.Client;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/items")]
    public class ItemsController : ControllerBase
    {
        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly DaprClient _daprClient;

        private readonly IUseCaseExecutor<GetCatalogItemByIdRequest> _getCatalogItemByIdUseCase;
        private readonly IUseCaseExecutor<CreateNewCatalogItemRequest> _createNewCatalogItemUseCase;
        private readonly IUseCaseExecutor<UpdateUnitPriceCatalogItemRequest> _updateUnitPriceCatalogUseCase;

        public ItemsController(ICatalogItemRepository catalogItemRepository,
            DaprClient daprClient,
            IUseCaseExecutor<CreateNewCatalogItemRequest> createNewCatalogItemUseCase,
            IUseCaseExecutor<UpdateUnitPriceCatalogItemRequest> updateUnitPriceCatalogUseCase,
            IUseCaseExecutor<GetCatalogItemByIdRequest> getCatalogItemByIdUseCase)
        {
            _catalogItemRepository = catalogItemRepository;
            _daprClient = daprClient;
            _createNewCatalogItemUseCase = createNewCatalogItemUseCase;
            _updateUnitPriceCatalogUseCase = updateUnitPriceCatalogUseCase;
            _getCatalogItemByIdUseCase = getCatalogItemByIdUseCase;
        }

        [HttpGet("{id}")]
        public ValueTask GetById(string id)
        {
            var response = _getCatalogItemByIdUseCase.SendAsync(new GetCatalogItemByIdRequest(id));
            return response.WriteResponseAsync(Response);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateItemRequest request)
        {
            var response =
                await _createNewCatalogItemUseCase.SendAsync(
                    new CreateNewCatalogItemRequest(request.Price, request.Name, request.Description));

            var raw = response.Content.GetRaw<CreateNewCatalogItemResponse>();
            return CreatedAtAction(nameof(GetById), new {id = raw.Id}, raw);
        }

        [HttpPut("{id}/unit-price")]
        public async Task<IActionResult> PutAsync(string id, decimal unitPrice)
        {
            var response =
                await _updateUnitPriceCatalogUseCase.SendAsync(new UpdateUnitPriceCatalogItemRequest(id, unitPrice));

            return NoContent();
        }
    }
}