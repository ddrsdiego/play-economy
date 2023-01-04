namespace Play.Customer.Core.Application.UseCases.GetCustomerById
{
    using Common.Application.UseCase;

    public sealed class GetCustomerByIdRequest : UseCaseRequest
    {
        public GetCustomerByIdRequest(string id) => Id = id;

        public string Id { get; }
    }
}