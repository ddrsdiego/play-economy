namespace Play.Inventory.Core.Application.Infra.Repositories.InventoryItemRepository
{
    using System.Runtime.CompilerServices;
    using CustomerRepository;
    using Domain.AggregateModel.InventoryItemAggregate;

    public static class InventoryItemEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static InventoryItemStateEntry ToStateEntry(this InventoryItem inventoryItemLine)
        {
            var itemsData = inventoryItemLine.Items
                .Select(item => new InventoryItemLineStateEntry
                {
                    CatalogItemId = item.CatalogItemId,
                    Quantity = item.Quantity,
                    AcquiredAt = item.AcquiredAt,
                });

            return new InventoryItemStateEntry
            {
                Id = inventoryItemLine.Customer.CustomerId,
                UserId = inventoryItemLine.Customer.CustomerId,
                Items = itemsData,
                CreatedAt = inventoryItemLine.CreatedAt
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static InventoryItem ToInventoryItem(this InventoryItemStateEntry inventoryItemStateEntry, CustomerStateEntry customerStateEntry)
        {
            var customer = customerStateEntry.ToCustomer();
            var inventoryItem = new InventoryItem(customer, inventoryItemStateEntry.CreatedAt);

            foreach (var item in inventoryItemStateEntry.Items)
            {
                inventoryItem.AddNewItemLine(new InventoryItemLine(item.CatalogItemId, item.Quantity, item.AcquiredAt));
            }

            return inventoryItem;
        }
    }
}