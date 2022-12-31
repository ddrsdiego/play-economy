namespace Play.Inventory.Core.Application.Infra.Clients
{
    using System.Net;
    using System.Text.Json;
    using CSharpFunctionalExtensions;

    public record GetCustomerByEmailResponse(string CustomerId, string Name, string Email);

    public interface ICustomerClient
    {
        Task<Result<GetCustomerByEmailResponse>> GetCustomerById(string userId);
    }

    public class CustomerClient : ICustomerClient
    {
        public const string PlayCustomerServiceName = "play-customer-service";
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Result<GetCustomerByEmailResponse>> GetCustomerById(string userId)
        {
            var client = _httpClientFactory.CreateClient(PlayCustomerServiceName);

            var response = await client.GetAsync($"/customers/{userId}");
            if (response.StatusCode != HttpStatusCode.OK)
                return Result.Failure<GetCustomerByEmailResponse>("");

            var content = await response.Content.ReadAsStringAsync();
            var customerResponse = JsonSerializer.Deserialize<GetCustomerByEmailResponse>(content,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            return Result.Success<GetCustomerByEmailResponse>(customerResponse);
        }
    }
}