﻿namespace Play.Inventory.Core.Application.Infra.Repositories.CustomerRepository
{
    using Common.Application.Infra.Repositories.Dapr;

    public sealed class CustomerStateEntry : IDaprStateEntry
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}