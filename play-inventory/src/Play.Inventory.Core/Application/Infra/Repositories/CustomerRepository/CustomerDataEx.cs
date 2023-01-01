namespace Play.Inventory.Core.Application.Infra.Repositories.CustomerRepository
{
    using Domain.AggregateModel.CustomerAggregate;

    public static class CustomerDataEx
    {
        public static CustomerStateEntry ToStateEntry(this Customer customer)
        {
            return new CustomerStateEntry
            {
                Id = customer.CustomerId,
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                CreatedAt = customer.CreatedAt
            };
        }

        public static Customer ToCustomer(this CustomerStateEntry stateEntry)
        {
            return new Customer(stateEntry.CustomerId, stateEntry.Name, stateEntry.Email, stateEntry.CreatedAt);
        }
    }
}