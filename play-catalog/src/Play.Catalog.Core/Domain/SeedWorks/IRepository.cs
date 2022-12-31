namespace Play.Catalog.Core.Domain.SeedWorks
{
    public interface IRepository<TEntity>
    {
        Task SaveOrUpdateAsync(TEntity entity);
        
        Task UpdateAsync(TEntity entity);
        
        Task<TEntity?> GetByIdAsync(string? id);
    }

    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}