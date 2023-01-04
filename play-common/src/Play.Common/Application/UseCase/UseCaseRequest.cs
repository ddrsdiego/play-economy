namespace Play.Common.Application.UseCase
{
    using System;

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
}