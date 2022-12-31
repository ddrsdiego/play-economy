namespace Play.Customer.Core.Domain.AggregateModel.CustomerAggregate
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICustomerRepository
    {
        Task<Customer> GetById(string customerId);

        Task<Customer> GetByDocumentAsync(string document);
        
        Task<Customer> GetByEmailAsync(string email);

        Task UpsertAsync(Customer customer, CancellationToken cancellationToken = default);
    }
}