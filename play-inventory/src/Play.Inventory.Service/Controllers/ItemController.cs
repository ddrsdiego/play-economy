namespace Play.Inventory.Service.Controllers
{
    using System.Threading.Tasks;
    using Common.Application.UseCase;
    using Core.Application.UseCases.GetInventoryItemByUserId;
    using Core.Application.UseCases.GrantItem;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IUseCaseExecutor<GrantItemRequest> _grantItemUseCase;
        private readonly IUseCaseExecutor<GetInventoryItemByUserIdReq> _getInventoryItemByUserIdUseCase;

        public ItemController(IUseCaseExecutor<GrantItemRequest> grantItemUseCase,
            IUseCaseExecutor<GetInventoryItemByUserIdReq> getInventoryItemByUserIdUseCase)
        {
            _grantItemUseCase = grantItemUseCase;
            _getInventoryItemByUserIdUseCase = getInventoryItemByUserIdUseCase;
        }

        [HttpGet("{userId}")]
        public ValueTask GetByUserId(string userId)
        {
            var response = _getInventoryItemByUserIdUseCase.SendAsync(new GetInventoryItemByUserIdReq(userId));
            return response.WriteResponseAsync(Response);
        }

        [HttpPost]
        public ValueTask Post([FromBody] GrantItemRequest request)
        {
            var response = _grantItemUseCase.SendAsync(request);
            return response.WriteResponseAsync(Response);
        }
    }
}