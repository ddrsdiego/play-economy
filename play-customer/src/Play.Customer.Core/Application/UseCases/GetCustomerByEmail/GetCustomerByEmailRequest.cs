namespace Play.Customer.Core.Application.UseCases.GetCustomerByEmail
{
    using Common.Application.UseCase;

    public sealed class GetCustomerByEmailRequest : UseCaseRequest
    {
        public GetCustomerByEmailRequest(string email) => Email = email;

        public readonly string Email;
    }
}