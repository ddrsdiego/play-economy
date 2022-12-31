namespace Play.Customer.Core.Application.UseCases.RegisterNewCustomer
{
    using System;

    public record RegisterNewCustomerResponse(string CustomerId, string Name, string Email, DateTimeOffset CreatedAt);
}