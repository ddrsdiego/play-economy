﻿namespace Play.Inventory.Core.Application.Infra.Repositories.CatalogItemRepository
{
    using Common.Application.Infra.Repositories.Dapr;

    public class CatalogItemStateEntry : IDaprStateEntry
    {
        public string Id { get; set; }
        public string CatalogItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}