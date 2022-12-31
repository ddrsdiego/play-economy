namespace Play.Inventory.Service.Controllers
{
    using System.Threading.Tasks;
    using Core.Application.Helpers.Constants;
    using Core.Domain.AggregateModel.CatalogItemAggregate;
    using Core.Domain.AggregateModel.CustomerAggregate;
    using Dapr;
    using Microsoft.AspNetCore.Mvc;

    public record CatalogItemCreated(string CatalogItemId, string Name, string Description);

    public record CatalogItemUpdated(string CatalogItemId, string Name, string Description);

    public record NewCustomerRegistered(string CustomerId, string Name, string Email);

    public record CustomerUpdated(string CustomerId, string Name, string Email);

    [ApiController]
    [Route("/")]
    public class SubscribeController : ControllerBase
    {
        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly ICustomerRepository _customerRepository;

        public SubscribeController(ICatalogItemRepository catalogItemRepository, ICustomerRepository customerRepository)
        {
            _catalogItemRepository = catalogItemRepository;
            _customerRepository = customerRepository;
        }

        [Topic("play-inventory-pub-sub", Topics.CatalogItemCreated)]
        [HttpPost(Topics.CatalogItemCreated)]
        public async Task<IActionResult> SubscriberToCatalogItemCreatedAsync(
            [FromBody] CatalogItemCreated catalogItemCreated)
        {
            var newCatalogItem = new CatalogItem(catalogItemCreated.CatalogItemId, catalogItemCreated.Name,
                catalogItemCreated.Description);

            await _catalogItemRepository.UpsertAsync(newCatalogItem);

            return Ok();
        }

        [Topic("play-inventory-pub-sub", Topics.CatalogItemUpdated)]
        [HttpPost(Topics.CatalogItemUpdated)]
        public async Task<IActionResult> SubscriberToCatalogItemUpdatedAsync(
            [FromBody] CatalogItemUpdated catalogItemUpdated)
        {
            var catalogItem = await _catalogItemRepository.GetByIdAsync(catalogItemUpdated.CatalogItemId);
            if (CatalogItem.Default.CatalogItemId == catalogItem.CatalogItemId)
            {
                catalogItem = new CatalogItem(catalogItemUpdated.CatalogItemId, catalogItemUpdated.Name,
                    catalogItemUpdated.Description);
            }
            else
            {
                catalogItem = new CatalogItem(catalogItem.CatalogItemId, catalogItemUpdated.Name,
                    catalogItemUpdated.Description);
            }

            await _catalogItemRepository.UpsertAsync(catalogItem);

            return Ok();
        }

        [Topic("play-inventory-pub-sub", Topics.CustomerUpdated)]
        [HttpPost(Topics.CustomerUpdated)]
        public async Task<IActionResult> SubscriberToCustomerUpdatedAsync([FromBody] CustomerUpdated customerUpdated)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerUpdated.CustomerId);
            if (customer.CustomerId == Customer.Default.CustomerId)
                return NoContent();

            customer = new Customer(customerUpdated.CustomerId, customerUpdated.Name, customerUpdated.Email);
            await _customerRepository.UpsertAsync(customer);

            return Ok();
        }
    }
}