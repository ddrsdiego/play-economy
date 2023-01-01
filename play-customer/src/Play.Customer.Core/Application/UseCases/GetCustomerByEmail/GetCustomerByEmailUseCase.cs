namespace Play.Customer.Core.Application.UseCases.GetCustomerByEmail
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Domain.AggregateModel.CustomerAggregate;

    public class GetCustomerByEmailUseCase : UseCaseExecutor<GetCustomerByEmailRequest, GetCustomerByEmailResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByEmailUseCase(ILoggerFactory logger, ICustomerRepository customerRepository)
            : base(logger.CreateLogger<GetCustomerByEmailUseCase>())
        {
            _customerRepository = customerRepository;
        }

        protected override async Task<GetCustomerByEmailResponse> ExecuteSendAsync(GetCustomerByEmailRequest request,
            CancellationToken token = default)
        {
            var customer = await _customerRepository.GetByEmailAsync(request.Email);

            var response =
                new GetCustomerByEmailResponse(customer.Identification.Id, customer.Name, customer.Email.Value, customer.CreatedAt);

            return response;
        }
    }
}