namespace Play.Inventory.Service.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Application.Requests;
    using Core.Application.Responses;
    using Core.Domain.AggregateModel.CatalogItemAggregate;
    using Core.Domain.AggregateModel.CustomerAggregate;
    using Core.Domain.AggregateModel.InventoryItemAggregate;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly ICustomerRepository _customerRepository;

        public ItemController(IInventoryItemRepository inventoryItemRepository,
            ICatalogItemRepository catalogItemRepository,
            ICustomerRepository customerRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _catalogItemRepository = catalogItemRepository;
            _customerRepository = customerRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var inventoryItem = await _inventoryItemRepository.GetByUserIdAsync(userId);
            if (inventoryItem.Customer.CustomerId.Equals(InventoryItem.Default.Customer.CustomerId))
                return NoContent();

            var catalogItemIds = inventoryItem.Items.Select(x => x.CatalogItemId).ToArray();
            var catalogItems = await _catalogItemRepository.GetByIdsAsync(catalogItemIds);

            var inventoryItems = inventoryItem.ToGetInventoryItemByUserIdResponse(catalogItems);
            return Ok(inventoryItems);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GrantItemRequest request)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(request.UserId);
            if (customer.CustomerId == Customer.Default.CustomerId)
                return BadRequest();

            var inventoryItem = await _inventoryItemRepository.GetByUserIdAsync(customer.CustomerId);
            if (inventoryItem.Customer.CustomerId.Equals(InventoryItem.Default.Customer.CustomerId))
            {
                inventoryItem = new InventoryItem(customer);
                var newItem = new InventoryItemLine(request.CatalogItemId, request.Quantity);

                inventoryItem.AddNewItemLine(newItem);
            }
            else
            {
                var inventoryItemUpdate = new InventoryItemLine(request.CatalogItemId, request.Quantity);
                inventoryItem.AddNewItemLine(inventoryItemUpdate);
            }

            await _inventoryItemRepository.SaveOrUpdateAsync(inventoryItem);
            return Ok();
        }
    }
}