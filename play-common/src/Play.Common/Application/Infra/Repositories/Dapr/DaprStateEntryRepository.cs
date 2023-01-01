namespace Play.Common.Application.Infra.Repositories.Dapr
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using CSharpFunctionalExtensions;
    using global::Dapr.Client;

    public abstract class DaprStateEntryRepository<TEntry> : IDaprStateEntryRepository<TEntry>
        where TEntry : IDaprStateEntry
    {
        private readonly DaprClient _daprClient;

        protected DaprStateEntryRepository(string stateStoreName, DaprClient daprClient)
        {
            StateStoreName = stateStoreName;
            _daprClient = daprClient;
        }

        public string StateStoreName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task UpsertAsync(TEntry entity, CancellationToken cancellationToken = default) =>
            UpsertAsync(new[] {entity}, cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task UpsertAsync(TEntry[] entities, CancellationToken cancellationToken = default) =>
            InternalUpsertAsync(entities, cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<TEntry>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await GetByIdAsync(new[] {id}, cancellationToken);
            return result[id];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IReadOnlyDictionary<string, Result<TEntry>>> GetByIdAsync(string[] ids,
            CancellationToken cancellationToken = default) =>
            InternalGetByIdAsync(ids, cancellationToken);

        private Task<IReadOnlyDictionary<string, Result<TEntry>>> InternalGetByIdAsync(IReadOnlyList<string> ids,
            CancellationToken cancellationToken = default)
        {
            var keys = new List<(string OriginalKey, string FormattedKey)>(ids.Count);
            var processorCount = Environment.ProcessorCount;

            for (var index = 0; index < ids.Count; index++)
            {
                var formattedKey = KeyFormatterHelper.ConstructStateStoreKey(typeof(TEntry).Name, ids[index]);
                keys.Add((ids[index], formattedKey));
            }

            var formattedKeys = keys.Select(x => x.FormattedKey).ToList();
            var bulkStateItemsTask = _daprClient.GetBulkStateAsync(StateStoreName, formattedKeys,
                parallelism: processorCount,
                cancellationToken: cancellationToken);

            var bulkStateItems = bulkStateItemsTask.IsCompletedSuccessfully
                ? bulkStateItemsTask.Result
                : SlowQuery(bulkStateItemsTask).Result;

            var results = new Dictionary<string, Result<TEntry>>(bulkStateItems.Count);
            foreach (var bulkStateItem in bulkStateItems)
            {
                var entity = JsonSerializer.Deserialize<TEntry>(bulkStateItem.Value);
                var key = keys.Single(x => x.FormattedKey == bulkStateItem.Key);
                if (entity == null)
                {
                    results.Add(key.OriginalKey, Result.Failure<TEntry>(""));
                    continue;
                }

                results.Add(key.OriginalKey, Result.Success(entity));
            }

            return Task.FromResult<IReadOnlyDictionary<string, Result<TEntry>>>(
                new ReadOnlyDictionary<string, Result<TEntry>>(results));

            static async Task<IReadOnlyList<BulkStateItem>> SlowQuery(Task<IReadOnlyList<BulkStateItem>> queryTask)
            {
                var bulkStateItem = await queryTask;
                return bulkStateItem;
            }
        }

        private Task InternalUpsertAsync(IReadOnlyCollection<TEntry> entities, CancellationToken cancellationToken)
        {
            var requests = new List<StateTransactionRequest>(entities.Count);

            foreach (var entity in entities)
            {
                var value = JsonSerializer.SerializeToUtf8Bytes(entity);
                var key = KeyFormatterHelper.ConstructStateStoreKey(typeof(TEntry).Name, entity.Id);

                requests.Add(new StateTransactionRequest(key, value, StateOperationType.Upsert));
            }

            var task = _daprClient.ExecuteStateTransactionAsync(StateStoreName, requests,
                cancellationToken: cancellationToken);

            return task.IsCompletedSuccessfully ? Task.CompletedTask : SlowExecute(task);

            static async Task SlowExecute(Task task) => await task;
        }
    }
}