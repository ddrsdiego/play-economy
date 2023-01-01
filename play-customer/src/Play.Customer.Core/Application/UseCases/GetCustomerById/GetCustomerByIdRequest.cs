namespace Play.Customer.Core.Application.UseCases.GetCustomerById
{
    public readonly struct GetCustomerByIdRequest
    {
        public GetCustomerByIdRequest(string id) => Id = id;

        public string Id { get; }
    }
}