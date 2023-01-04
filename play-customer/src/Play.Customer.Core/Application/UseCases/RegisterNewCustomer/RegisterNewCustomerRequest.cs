namespace Play.Customer.Core.Application.UseCases.RegisterNewCustomer
{
    using Common.Application.UseCase;
    
    public sealed class RegisterNewCustomerRequest : UseCaseRequest
    {
        public RegisterNewCustomerRequest(string document, string name, string email)
        {
            Document = document;
            Name = name;
            Email = email;
        }

        public readonly string Document;
        public readonly string Name;
        public readonly string Email;
    }
}