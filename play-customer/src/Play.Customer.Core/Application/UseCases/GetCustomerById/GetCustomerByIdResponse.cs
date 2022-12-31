namespace Play.Customer.Core.Application.UseCases.GetCustomerById
{
    using System;
    using GetCustomerByEmail;

    public sealed class GetCustomerByIdResponse : GetCustomerResponse
    {
        public GetCustomerByIdResponse(string customerId, string name, string email, DateTimeOffset createdAt)
            : base(customerId, name, email, createdAt)
        {
        }
    }
}