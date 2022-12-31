namespace Play.Customer.Core.Application.UseCases.GetCustomerByEmail
{
    using System;

    public abstract class GetCustomerResponse
    {
        protected GetCustomerResponse(string customerId, string name, string email, DateTimeOffset createdAt)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            CreatedAt = createdAt;
        }

        public string CustomerId { get; }
        public string Name { get; }
        public string Email { get; }
        public DateTimeOffset CreatedAt { get; }
    }
    
    public sealed class GetCustomerByEmailResponse : GetCustomerResponse
    {
        public GetCustomerByEmailResponse(string customerId, string name, string email, DateTimeOffset createdAt)
            :base(customerId, name, email, createdAt)
        {
        }
    }
}