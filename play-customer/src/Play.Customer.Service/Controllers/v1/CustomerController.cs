namespace Play.Customer.Service.Controllers.v1
{
    using System.Threading.Tasks;
    using Common.Application;
    using Common.Application.UseCase;
    using Microsoft.AspNetCore.Mvc;
    using Core.Application.UseCases.GetCustomerById;
    using Core.Application.UseCases.RegisterNewCustomer;
    using Core.Application.UseCases.UpdateCustomer;
    using Google.Protobuf.WellKnownTypes;

    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IUseCaseExecutor<UpdateCustomerRequest> _updateCustomerUseCase;
        private readonly IUseCaseExecutor<GetCustomerByIdRequest> _getCustomerByIdUseCase;
        private readonly IUseCaseExecutor<RegisterNewCustomerRequest> _registerNewCustomerUseCase;

        public CustomerController(IUseCaseExecutor<RegisterNewCustomerRequest> registerNewCustomerUseCase,
            IUseCaseExecutor<UpdateCustomerRequest> updateCustomerUseCase,
            IUseCaseExecutor<GetCustomerByIdRequest> getCustomerByIdUseCase)
        {
            _registerNewCustomerUseCase = registerNewCustomerUseCase;
            _updateCustomerUseCase = updateCustomerUseCase;
            _getCustomerByIdUseCase = getCustomerByIdUseCase;
        }

        [HttpGet("{id}")]
        public ValueTask GetById(string id)
        {
            var responseTask = _getCustomerByIdUseCase.SendAsync(new GetCustomerByIdRequest(id));
            return responseTask.WriteResponseAsync(Response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewCustomer([FromBody] RegisterNewCustomerRequest request)
        {
            var response = await _registerNewCustomerUseCase.SendAsync(request);

            var registerResponse = response.Content.GetRaw<RegisterNewCustomerResponse>();
            return CreatedAtAction(nameof(GetById), new
            {
                id = registerResponse.CustomerId
            }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(string id, [FromBody] UpdateCustomer request)
        {
            var response =
                await _updateCustomerUseCase.SendAsync(new UpdateCustomerRequest(id, request.Name));

            return NoContent();
        }

        public class UpdateCustomer
        {
            public string Name { get; set; }
        }
    }
}