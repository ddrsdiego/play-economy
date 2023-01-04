namespace Play.Common.Application.Infra.Repositories.Dapr
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using CSharpFunctionalExtensions;

    public interface IDaprStateEntry
    {
        string StateEntryKey { get; }
    }

    public abstract class DaprStateEntry : IDaprStateEntry
    {
        
        protected DaprStateEntry(string stateEntryKey)
        {
            StateEntryKey = stateEntryKey;
        }

        public string StateEntryKey { get; protected set; }
    }

    public interface IDaprStateEntryRepository<TEntry>
        where TEntry : IDaprStateEntry
    {
        Task UpsertAsync(TEntry entity, CancellationToken cancellationToken = default);

        Task UpsertAsync(TEntry[] entities, CancellationToken cancellationToken = default);

        Task<Result<TEntry>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<IReadOnlyDictionary<string, Result<TEntry>>> GetByIdAsync(string[] ids,
            CancellationToken cancellationToken = default);
    }
}