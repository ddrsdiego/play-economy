namespace Play.Inventory.Service.Controllers.v1
{
    using System.Threading.Tasks;
    using Common.Application.UseCase;
    using Dapr;
    using Microsoft.AspNetCore.Mvc;
    using Play.Common.Application.Infra.Repositories.Dapr;
    using Core.Application.Helpers.Constants;
    using Core.Application.Infra.Repositories.CatalogItemRepository;
    using Core.Application.Infra.Repositories.CustomerRepository;
    using Core.Application.UseCases.CreateCatalogItem;
    using Core.Domain.AggregateModel.CatalogItemAggregate;
    using Core.Domain.AggregateModel.CustomerAggregate;

    public record CatalogItemCreated(string CatalogItemId, string Name, string Description);

    public record CatalogItemUpdated(string CatalogItemId, string Name, string Description);

    public record NewCustomerRegistered(string CustomerId, string Name, string Email);

    public record CustomerUpdated(string CustomerId, string Name, string Email);

    [ApiController]
    [Route("/")]
    public class SubscribeController : ControllerBase
    {
        private readonly IUseCaseExecutor<CreateCatalogItemReq> _createCatalogItemUseCase;
        private readonly IDaprStateEntryRepository<CustomerData> _customerDaprRepository;
        private readonly IDaprStateEntryRepository<CatalogItemData> _catalogItemDaprRepository;

        public SubscribeController(IDaprStateEntryRepository<CatalogItemData> catalogItemDaprRepository,
            IDaprStateEntryRepository<CustomerData> customerDaprRepository,
            IUseCaseExecutor<CreateCatalogItemReq> createCatalogItemUseCase)
        {
            _catalogItemDaprRepository = catalogItemDaprRepository;
            _customerDaprRepository = customerDaprRepository;
            _createCatalogItemUseCase = createCatalogItemUseCase;
        }

        [Topic("play-inventory-pub-sub", Topics.CatalogItemCreated)]
        [HttpPost(Topics.CatalogItemCreated)]
        public async Task<IActionResult> SubscriberToCatalogItemCreatedAsync(
            [FromBody] CatalogItemCreated catalogItemCreated)
        {
            var newCatalogItem = new CatalogItem(catalogItemCreated.CatalogItemId, catalogItemCreated.Name,
                catalogItemCreated.Description);

            await _catalogItemDaprRepository.UpsertAsync(newCatalogItem.ToCatalogItemData());

            return Ok();
        }

        [Topic("play-inventory-pub-sub", Topics.CatalogItemUpdated)]
        [HttpPost(Topics.CatalogItemUpdated)]
        public ValueTask SubscriberToCatalogItemUpdatedAsync(
            [FromBody] CatalogItemUpdated catalogItemUpdated)
        {
            var response = _createCatalogItemUseCase.SendAsync(new CreateCatalogItemReq(
                catalogItemUpdated.CatalogItemId,
                catalogItemUpdated.Name,
                catalogItemUpdated.Description
            ));
            return response.WriteResponseAsync(Response);
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