namespace Play.Customer.Core.Application.UseCases.RegisterNewCustomer
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.AggregateModel.CustomerAggregate;
    using Microsoft.Extensions.Logging;

    public sealed class
        RegisterNewCustomerUseCase : UseCaseExecutor<RegisterNewCustomerRequest, RegisterNewCustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public RegisterNewCustomerUseCase(ILoggerFactory logger, ICustomerRepository customerRepository)
            : base(logger.CreateLogger<RegisterNewCustomerUseCase>())
        {
            _customerRepository = customerRepository;
        }

        protected override async Task<RegisterNewCustomerResponse> ExecuteSendAsync(RegisterNewCustomerRequest request,
            CancellationToken token = default)
        {
            if (await TryFindAlreadyRegisteredCustomers(request))
            {
                return new RegisterNewCustomerResponse(string.Empty, string.Empty, string.Empty,
                    default);
            }

            var newCustomer = new Customer(request.Document, request.Name, request.Email);
            await _customerRepository.UpsertAsync(newCustomer, token);

            return new RegisterNewCustomerResponse(newCustomer.Identification.Id, newCustomer.Name, newCustomer.Email.Value,
                newCustomer.CreatedAt);
        }

        private async Task<bool> TryFindAlreadyRegisteredCustomers(RegisterNewCustomerRequest request)
        {
            var getByEmailTask = _customerRepository.GetByEmailAsync(request.Email);
            var getByDocumentTask = _customerRepository.GetByEmailAsync(request.Document);

            var tasks = new List<Task<Customer>>(2) {getByEmailTask, getByDocumentTask};

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                var customer = await task;
                
                if (customer.IsValidCustomer)
                    return customer.IsValidCustomer;
            }

            return false;
        }
    }
}