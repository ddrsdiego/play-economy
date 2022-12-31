namespace Play.Inventory.Core.Application.Infra.Repositories
{
    using System.Runtime.CompilerServices;
    using Domain.AggregateModel.InventoryItemAggregate;

    internal static class InventoryItemEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static InventoryItemData ToInventoryItemData(this InventoryItem inventoryItemLine)
        {
            var itemsData = inventoryItemLine.Items
                .Select(item => new InventoryItemLineData
                {
                    CatalogItemId = item.CatalogItemId,
                    Quantity = item.Quantity,
                    AcquiredAt = item.AcquiredAt,
                });

            return new InventoryItemData
            {
                UserId = inventoryItemLine.Customer.CustomerId,
                Items = itemsData,
                CreatedAt = inventoryItemLine.CreatedAt
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static InventoryItem ToInventoryItem(this InventoryItemData inventoryItemData, CustomerData customerData)
        {
            var customer = customerData.ToCustomer();
            var inventoryItem = new InventoryItem(customer, inventoryItemData.CreatedAt);

            foreach (var item in inventoryItemData.Items)
            {
                inventoryItem.AddNewItemLine(new InventoryItemLine(item.CatalogItemId, item.Quantity, item.AcquiredAt));
            }

            return inventoryItem;
        }
    }
}