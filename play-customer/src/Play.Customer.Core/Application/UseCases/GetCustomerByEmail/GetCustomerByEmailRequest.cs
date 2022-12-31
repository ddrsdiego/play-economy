namespace Play.Customer.Core.Application.UseCases.GetCustomerByEmail
{
    public readonly struct GetCustomerByEmailRequest
    {
        public GetCustomerByEmailRequest(string email) => Email = email;

        public readonly string Email;
    }
}