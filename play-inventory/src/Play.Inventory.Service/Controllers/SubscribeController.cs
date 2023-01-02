namespace Play.Inventory.Service.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Common.Application.Infra.Repositories.Dapr;
    using Core.Application.Helpers.Constants;
    using Core.Application.Infra.Repositories.CatalogItemRepository;
    using Core.Application.Infra.Repositories.CustomerRepository;
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
        private readonly IDaprStateEntryRepository<CustomerData> _customerDaprRepository;
        private readonly IDaprStateEntryRepository<CatalogItemData> _catalogItemDaprRepository;

        public SubscribeController(IDaprStateEntryRepository<CatalogItemData> catalogItemDaprRepository,
            IDaprStateEntryRepository<CustomerData> customerDaprRepository)
        {
            _catalogItemDaprRepository = catalogItemDaprRepository;
            _customerDaprRepository = customerDaprRepository;
        }

        [Topic("play-inventory-pub-sub", Topics.CatalogItemCreated)]
        [HttpPost(Topics.CatalogItemCreated)]
        public async Task<IActionResult> SubscriberToCatalogItemCreatedAsync(
            [FromBody] CatalogItemCreated catalogItemCreated)
        {
            var newCatalogItem = new CatalogItem(catalogItemCreated.CatalogItemId, catalogItemCreated.Name,
                catalogItemCreated.Description);

            await _catalogItemDaprRepository.UpsertAsync(newCatalogItem.ToCatalogItem());

            return Ok();
        }

        [Topic("play-inventory-pub-sub", Topics.CatalogItemUpdated)]
        [HttpPost(Topics.CatalogItemUpdated)]
        public async Task<IActionResult> SubscriberToCatalogItemUpdatedAsync(
            [FromBody] CatalogItemUpdated catalogItemUpdated)
        {
            CatalogItemData catalogItemData;

            var catalogItemDataResult = await _catalogItemDaprRepository.GetByIdAsync(catalogItemUpdated.CatalogItemId);
            if (catalogItemDataResult.IsFailure)
            {
                catalogItemData = new CatalogItemData
                {
                    Id = catalogItemUpdated.CatalogItemId,
                    CatalogItemId = catalogItemUpdated.CatalogItemId,
                    Name = catalogItemUpdated.Name,
                    Description = catalogItemUpdated.Description,
                    CreatedAt = DateTimeOffset.UtcNow
                };
            }
            else
            {
                catalogItemData = new CatalogItemData
                {
                    Id = catalogItemUpdated.CatalogItemId,
                    CatalogItemId = catalogItemDataResult.Value.CatalogItemId,
                    Name = catalogItemDataResult.Value.Name,
                    Description = catalogItemDataResult.Value.Description,
                    Updated = DateTimeOffset.UtcNow
                };
            }

            await _catalogItemDaprRepository.UpsertAsync(catalogItemData);

            return Ok();
        }

        [Topic("play-inventory-pub-sub", Topics.CustomerUpdated)]
        [HttpPost(Topics.CustomerUpdated)]
        public async Task<IActionResult> SubscriberToCustomerUpdatedAsync([FromBody] CustomerUpdated customerUpdated)
        {
            var customerResult = await _customerDaprRepository.GetByIdAsync(customerUpdated.CustomerId);
            if (customerResult.IsFailure)
                return NoContent();

            var newCustomer = new Customer(customerUpdated.CustomerId, customerUpdated.Name, customerUpdated.Email);
            await _customerDaprRepository.UpsertAsync(newCustomer.ToStateEntry());

            return Ok();
        }
    }
}