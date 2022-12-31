namespace Play.Customer.Service.Controllers
{
    using System.Threading.Tasks;
    using Core.Application.UseCases;
    using Core.Application.UseCases.GetCustomerById;
    using Core.Application.UseCases.RegisterNewCustomer;
    using Core.Application.UseCases.UpdateCustomer;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IUseCaseExecutor<RegisterNewCustomerRequest, RegisterNewCustomerResponse>
            _registerNewCustomerUseCase;

        private readonly IUseCaseExecutor<UpdateCustomerRequest, UpdateCustomerResponse> _updateCustomerUseCase;
        private readonly IUseCaseExecutor<GetCustomerByIdRequest, GetCustomerByIdResponse> _getCustomerByIdUseCase;

        public CustomerController(
            IUseCaseExecutor<RegisterNewCustomerRequest, RegisterNewCustomerResponse> registerNewCustomerUseCase,
            IUseCaseExecutor<UpdateCustomerRequest, UpdateCustomerResponse> updateCustomerUseCase,
            IUseCaseExecutor<GetCustomerByIdRequest, GetCustomerByIdResponse> getCustomerByIdUseCase)
        {
            _registerNewCustomerUseCase = registerNewCustomerUseCase;
            _updateCustomerUseCase = updateCustomerUseCase;
            _getCustomerByIdUseCase = getCustomerByIdUseCase;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _getCustomerByIdUseCase.SendAsync(new GetCustomerByIdRequest(id));
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewCustomer([FromBody] RegisterNewCustomerRequest request)
        {
            var response = await _registerNewCustomerUseCase.SendAsync(request);

            return CreatedAtAction(nameof(GetById), new {id = response.CustomerId}, response);
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