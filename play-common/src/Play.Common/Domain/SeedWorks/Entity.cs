namespace Play.Common.Domain.SeedWorks
{
    using System;

    public abstract class Entity
    {
        protected Entity()
            :this(Guid.NewGuid().ToString())
        {
        }
        
        protected Entity(string id)
        {
            Id = id;
        }
        
        public string Id { get; }
    }
}