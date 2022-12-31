namespace Play.Customer.Core.Application.UseCases
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public interface IUseCaseExecutor<in TRequest, TResponse>
    {
        Task<TResponse> SendAsync(TRequest request, CancellationToken token = default);
    }

    public abstract class UseCaseExecutor<TRequest, TResponse> : IUseCaseExecutor<TRequest, TResponse>
    {
        protected UseCaseExecutor(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }

        public async Task<TResponse> SendAsync(TRequest request, CancellationToken token = default)
        {
            TResponse response;

            try
            {
                response = await ExecuteSendAsync(request, token);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "");
                throw;
            }

            return response;
        }

        protected abstract Task<TResponse> ExecuteSendAsync(TRequest request, CancellationToken token = default);
    }
}