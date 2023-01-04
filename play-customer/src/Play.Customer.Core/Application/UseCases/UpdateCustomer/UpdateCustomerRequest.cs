namespace Play.Customer.Core.Application.UseCases.UpdateCustomer
{
    using Common.Application.UseCase;

    public sealed class UpdateCustomerRequest : UseCaseRequest
    {
        public UpdateCustomerRequest(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public readonly string Id;
        public readonly string Name;
    }
}