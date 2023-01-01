namespace Play.Common.UnitTests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application;
    using Application.UseCase;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class RegisterNewCustomerRequest : UseCaseRequest
    {
    }
    
    public sealed class RegisterNewCustomerUseCase : UseCaseExecutor<RegisterNewCustomerRequest>
    {
        public RegisterNewCustomerUseCase(ILoggerFactory logger)
            : base(logger.CreateLogger<RegisterNewCustomerUseCase>())
        {
        }

        protected override Task<Response> ExecuteSendAsync(RegisterNewCustomerRequest request,
            CancellationToken token = default)
        {
            return Task.FromResult(Response.Ok(StatusCodes.Status201Created, request.RequestId));
        }
    }
}