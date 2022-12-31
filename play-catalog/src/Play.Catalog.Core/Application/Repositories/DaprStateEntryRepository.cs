namespace Play.Catalog.Core.Application.Repositories
{
    using Dapr.Client;
    using Domain.SeedWorks;

    public sealed class DaprStateEntryRepository<T> : IRepository<T> where T : Entity
    {
        private readonly string _stateStoreName;
        private readonly DaprClient _daprClient;

        public DaprStateEntryRepository(string stateStoreName, DaprClient daprClient)
        {
            _stateStoreName = stateStoreName;
            _daprClient = daprClient;
        }

        public Task SaveOrUpdateAsync(T entity)
        {
            var stateStoreKey = KeyFormatterHelper.CreateStateStoreKey(entity.GetType().Name, entity.Id);

            try
            {
                var slowSaveStateAsync = _daprClient.SaveStateAsync(_stateStoreName, stateStoreKey,
                    entity);

                return slowSaveStateAsync.IsCompletedSuccessfully
                    ? Task.CompletedTask
                    : SlowSaveAsync(slowSaveStateAsync);
            }
            catch (Exception e)
            {
                throw;
            }

            static async Task SlowSaveAsync(Task task) => await task;
        }

        public Task UpdateAsync(T entity)
        {
            var stateStoreKey = KeyFormatterHelper.CreateStateStoreKey(nameof(entity), entity.Id);
            
            var slowUpdateAsync = _daprClient.SaveStateAsync(_stateStoreName, stateStoreKey,
                entity);

            return slowUpdateAsync.IsCompletedSuccessfully ? Task.CompletedTask : SlowSaveAsync(slowUpdateAsync);

            static async Task SlowSaveAsync(Task task) => await task;
        }

        public async Task<T> GetByIdAsync(string? id)
        {
            var stateStoreKey = KeyFormatterHelper.CreateStateStoreKey(typeof(T).Name, id);

            try
            {
                var data = await _daprClient.GetStateEntryAsync<T>(_stateStoreName, stateStoreKey);
                return data.Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}