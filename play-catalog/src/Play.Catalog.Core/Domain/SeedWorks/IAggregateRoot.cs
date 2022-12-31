namespace Play.Catalog.Core.Domain.SeedWorks
{
    public interface IAggregateRoot
    {
    }

    public abstract class Entity
    {
        protected Entity(string? id)
        {
            Id = id;
        }
        
        public string? Id { get; }
    }
}