namespace Play.Customer.Core.Application.UseCases.GetCustomerById
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.AggregateModel.CustomerAggregate;
    using Microsoft.Extensions.Logging;

    public sealed class GetCustomerByIdUseCase : UseCaseExecutor<GetCustomerByIdRequest, GetCustomerByIdResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdUseCase(ILoggerFactory logger, ICustomerRepository customerRepository)
            : base(logger.CreateLogger<GetCustomerByIdUseCase>())
        {
            _customerRepository = customerRepository;
        }

        protected override async Task<GetCustomerByIdResponse> ExecuteSendAsync(GetCustomerByIdRequest request,
            CancellationToken token = default)
        {
            var customer = await _customerRepository.GetById(request.Id);

            var response =
                new GetCustomerByIdResponse(customer.Identification.Id, customer.Name, customer.Email.Value,
                    customer.CreatedAt);

            return response;
        }
    }
}