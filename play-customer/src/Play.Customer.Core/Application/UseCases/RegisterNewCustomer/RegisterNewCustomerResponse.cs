namespace Play.Customer.Core.Application.UseCases.RegisterNewCustomer
{
    using System;

    public readonly struct RegisterNewCustomerResponse
    {
        public string CustomerId { get; }
        public string Name { get; }
        public string Email { get; }
        public DateTimeOffset CreatedAt { get; }

        public RegisterNewCustomerResponse(string customerId, string name, string email, DateTimeOffset createdAt)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            CreatedAt = createdAt;
        }
    }
}