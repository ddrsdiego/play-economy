namespace Play.Customer.Core.Application.UseCases.UpdateCustomer
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dapr.Client;
    using Domain.AggregateModel.CustomerAggregate;
    using Helpers.Constants;
    using Microsoft.Extensions.Logging;

    public sealed record UpdateCustomerRequest(string Id, string Name);

    public class UpdateCustomerResponse
    {
    }

    public class UpdateCustomerUseCase : UseCaseExecutor<UpdateCustomerRequest, UpdateCustomerResponse>
    {
        private readonly DaprClient _daprClient;
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerUseCase(ILoggerFactory logger, ICustomerRepository customerRepository,
            DaprClient daprClient)
            : base(logger.CreateLogger<UpdateCustomerUseCase>())
        {
            _customerRepository = customerRepository;
            _daprClient = daprClient;
        }

        protected override async Task<UpdateCustomerResponse> ExecuteSendAsync(UpdateCustomerRequest request,
            CancellationToken token = default)
        {
            var customer = await _customerRepository.GetById(request.Id);

            customer.UpdateName(request.Name);

            await _customerRepository.UpsertAsync(customer, token);

            var customerUpdated = new CustomerUpdated(customer.Identification.Id, customer.Name, customer.Email.Value);

            _ = _daprClient.PublishEventAsync("play-customer-pub-sub", Topics.CustomerUpdated,
                customerUpdated, token);

            return new UpdateCustomerResponse();
        }
    }
}