namespace Play.Inventory.Service.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Application.Infra.Repositories.Dapr;
    using Common.Application.UseCase;
    using Core.Application.Infra.Repositories;
    using Core.Application.Infra.Repositories.CatalogItemRepository;
    using Core.Application.Infra.Repositories.CustomerRepository;
    using Core.Application.Infra.Repositories.InventoryItemRepository;
    using Core.Application.Responses;
    using Core.Application.UseCases.GrantItem;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IUseCaseExecutor<GrantItemRequest> _grantItemUseCase;
        private readonly IDaprStateEntryRepository<CustomerStateEntry> _customerDaprRepository;
        private readonly IDaprStateEntryRepository<CatalogItemStateEntry> _catalogItemDaprRepository;
        private readonly IDaprStateEntryRepository<InventoryItemStateEntry> _inventoryRepository;

        public ItemController(IDaprStateEntryRepository<CatalogItemStateEntry> catalogItemDaprRepository,
            IDaprStateEntryRepository<CustomerStateEntry> customerDaprRepository,
            IUseCaseExecutor<GrantItemRequest> grantItemUseCase,
            IDaprStateEntryRepository<InventoryItemStateEntry> inventoryRepository)
        {
            _catalogItemDaprRepository = catalogItemDaprRepository;
            _customerDaprRepository = customerDaprRepository;
            _grantItemUseCase = grantItemUseCase;
            _inventoryRepository = inventoryRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var getCustomerTask = _customerDaprRepository.GetByIdAsync(userId);
            var getInventoryTask = _inventoryRepository.GetByIdAsync(userId);

            await Task.WhenAll(getCustomerTask, getInventoryTask);

            var inventoryItemResult = await getInventoryTask;
            if (inventoryItemResult.IsFailure)
                return NoContent();

            var customerResult = await getCustomerTask;
            var inventoryItem = inventoryItemResult.Value.ToInventoryItem(customerResult.Value);

            var catalogItemIds = inventoryItem.Items.Select(x => x.CatalogItemId).ToArray();
            var catalogItemsData = await _catalogItemDaprRepository.GetByIdAsync(catalogItemIds);

            var catalogItems = catalogItemsData
                .Select(x => x.Value.Value.ToStateEntry()).ToList().AsReadOnly();

            var inventoryItems = inventoryItem.ToGetInventoryItemByUserIdResponse(catalogItems);
            return Ok(inventoryItems);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GrantItemRequest request)
        {
            var response = await _grantItemUseCase.SendAsync(request);
            return Ok();
        }
    }
}