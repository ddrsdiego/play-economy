namespace Play.Catalog.Core.Domain.AggregatesModel.CatalogItemAggregate
{
    using SeedWorks;

    public sealed class CatalogItem : Entity, IAggregateRoot
    {
        private CatalogItem()
            : base(string.Empty)
        {
        }

        public CatalogItem(decimal price, string? name, string? description)
            : this(Guid.NewGuid().ToString().Split('-')[0], price, name, description, DateTimeOffset.UtcNow)
        {
        }

        internal CatalogItem(string? id, decimal price, string? name, string? description, DateTimeOffset createAt)
            : base(id)
        {
            Price = new UnitPrice(price);
            Description = new CatalogItemDescription(name, description);
            CreateAt = createAt;
        }

        public static CatalogItem Default => new();
        public CatalogItemDescription Description { get; private set; }
        public UnitPrice Price { get; private set; }
        public DateTimeOffset CreateAt { get; }

        public void UpdateDescription(CatalogItemDescription newDescription)
        {
            Description = newDescription;
        }

        public void UpdateUnitePrice(decimal price) => UpdateUnitePrice(new UnitPrice(price));

        public void UpdateUnitePrice(UnitPrice newUnitPrice)
        {
            Price = newUnitPrice;
        }
    }
}