namespace Play.Common.Application.UseCase
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class UseCaseRequest
    {
        protected UseCaseRequest()
            : this(Guid.NewGuid().ToString().Split('-')[0])
        {
        }

        protected UseCaseRequest(string requestId)
        {
            RequestAt = DateTime.UtcNow;
            RequestId = requestId;
        }

        public string RequestId { get; }

        public DateTime RequestAt { get; }
    }

    public interface IUseCaseExecutor<in TRequest>
        where TRequest : UseCaseRequest
    {
        Task<Response> SendAsync(TRequest request, CancellationToken token = default);
    }
}