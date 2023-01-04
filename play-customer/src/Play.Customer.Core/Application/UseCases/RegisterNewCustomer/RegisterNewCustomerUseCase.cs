namespace Play.Customer.Core.Application.UseCases.RegisterNewCustomer
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Application;
    using Domain.AggregateModel.CustomerAggregate;
    using Microsoft.Extensions.Logging;
    using Play.Common.Application.UseCase;

    internal sealed class RegisterNewCustomerUseCase : UseCaseExecutor<RegisterNewCustomerRequest>
    {
        private readonly ICustomerRepository _customerRepository;

        public RegisterNewCustomerUseCase(ILoggerFactory logger, ICustomerRepository customerRepository)
            : base(logger.CreateLogger<RegisterNewCustomerUseCase>())
        {
            _customerRepository = customerRepository;
        }

        protected override async Task<Response> ExecuteSendAsync(RegisterNewCustomerRequest request,
            CancellationToken token = default)
        {
            if (await TryFindAlreadyRegisteredCustomers(request))
            {
                Logger.LogWarning("");
                return Response.Fail("ERROR_USER_ALREADY_REGISTERED", "User already registered.");
            }

            var newCustomer = new Customer(request.Document, request.Name, request.Email);
            await _customerRepository.UpsertAsync(newCustomer, token);

            var responseContent = new RegisterNewCustomerResponse(newCustomer.Identification.Id, newCustomer.Name,
                newCustomer.Email.Value,
                newCustomer.CreatedAt);

            return Response.Ok(ResponseContent.Create(responseContent));
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