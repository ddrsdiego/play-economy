namespace Play.Customer.Core.Application.UseCases.RegisterNewCustomer
{
    public record RegisterNewCustomerRequest(string Document, string Name, string Email);
}