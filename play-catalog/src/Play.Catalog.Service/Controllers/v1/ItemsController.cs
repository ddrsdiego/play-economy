namespace Play.Catalog.Service.Controllers.v1
{
    using System.Threading.Tasks;
    using Common.Application.UseCase;
    using Core.Application.Requests;
    using Core.Application.UseCases.CreateNewCatalogItem;
    using Core.Application.UseCases.GetCatalogItemById;
    using Core.Application.UseCases.UpdateUnitPriceCatalogItem;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IUseCaseExecutor<GetCatalogItemByIdRequest> _getCatalogItemByIdUseCase;
        private readonly IUseCaseExecutor<CreateNewCatalogItemRequest> _createNewCatalogItemUseCase;
        private readonly IUseCaseExecutor<UpdateUnitPriceCatalogItemRequest> _updateUnitPriceCatalogUseCase;

        public ItemsController(IUseCaseExecutor<CreateNewCatalogItemRequest> createNewCatalogItemUseCase,
            IUseCaseExecutor<UpdateUnitPriceCatalogItemRequest> updateUnitPriceCatalogUseCase,
            IUseCaseExecutor<GetCatalogItemByIdRequest> getCatalogItemByIdUseCase)
        {
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

            var createNewCatalogItemResponse = response.Content.GetRaw<CreateNewCatalogItemResponse>();
            
            return CreatedAtAction(nameof(GetById), new {id = createNewCatalogItemResponse.Id},
                createNewCatalogItemResponse);
        }

        [HttpPut("{id}/unit-price")]
        public ValueTask PutAsync(string id, decimal unitPrice)
        {
            var response =
                _updateUnitPriceCatalogUseCase.SendAsync(new UpdateUnitPriceCatalogItemRequest(id, unitPrice));

            return response.WriteResponseAsync(Response);
        }
    }
}