namespace Play.Customer.Core.Domain.AggregateModel.CustomerAggregate
{
    using System;

    public sealed class Customer
    {
        private Customer()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, default)
        {
        }

        public Customer(string document, string name, string email)
            : this(Guid.NewGuid().ToString().Split('-')[0], document, name, email, DateTimeOffset.UtcNow)
        {
        }

        internal Customer(string customerId, string document, string name, string email, DateTimeOffset createdAt)
        {
            Document = document;
            Email = new Email(email);
            Identification = new CustomerIdentification(customerId, email, document);
            Name = name;
            CreatedAt = createdAt;
        }

        public static Customer Default => new();

        public bool IsValidCustomer => !string.IsNullOrEmpty(Identification.Value);

        public CustomerIdentification Identification { get; }
        public string Document { get; }
        public string Name { get; private set; }
        public Email Email { get; }
        public DateTimeOffset CreatedAt { get; }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}