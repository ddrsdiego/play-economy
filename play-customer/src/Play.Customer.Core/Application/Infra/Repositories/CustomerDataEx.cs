namespace Play.Customer.Core.Application.Infra.Repositories
{
    using Domain.AggregateModel.CustomerAggregate;

    internal static class CustomerDataEx
    {
        public static Customer ToCustomer(this CustomerData data)
        {
            return new Customer(data.Properties.CustomerId, data.Properties.Document, data.Properties.Name,
                data.Properties.Email,
                data.Properties.CreatedAt);
        }

        public static CustomerData ToCustomerData(this Customer customer)
        {
            return new CustomerData
            {
                Id = customer.Identification.Value,
                Properties = new CustomerPropertiesData
                {
                    CustomerId = customer.Identification.Id,
                    Document = customer.Document,
                    Name = customer.Name,
                    Email = customer.Email,
                    CreatedAt = customer.CreatedAt
                }
            };
        }
    }
}