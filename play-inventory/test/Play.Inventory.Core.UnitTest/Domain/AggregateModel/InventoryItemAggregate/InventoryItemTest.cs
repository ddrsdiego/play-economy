namespace Play.Inventory.Core.UnitTest.Domain.AggregateModel.InventoryItemAggregate
{
    using Core.Domain.AggregateModel.CustomerAggregate;
    using Core.Domain.AggregateModel.InventoryItemAggregate;
    using FluentAssertions;
    using Xunit;

    public class InventoryItemTest
    {
        [Fact]
        public void Should_Create_Valid_Empty_InventoryItem()
        {
            var userId = Guid.NewGuid().ToString();
            var customer = new Customer(userId, "customer-test", "customer-email-test");
            var inventoryItem = new InventoryItem(customer);

            inventoryItem.Customer.CustomerId.Should().Be(userId);
            inventoryItem.Items.Count.Should().Be(0);
        }

        [Fact]
        public void Should_Create_Valid_Empty_InventoryItem_1()
        {
            var userId = Guid.NewGuid().ToString();
            var customer = new Customer(userId, "customer-test", "customer-email-test");
            var inventoryItem = new InventoryItem(customer);

            var catalogItemId = Guid.NewGuid().ToString();
            var item = new InventoryItemLine(catalogItemId, 1);

            inventoryItem.AddNewItemLine(item);

            inventoryItem.Customer.CustomerId.Should().Be(userId);
            inventoryItem.Items.Count.Should().Be(1);
        }

        [Fact]
        public void Should_Create_Valid_Empty_InventoryItem_2()
        {
            var userId = Guid.NewGuid().ToString();
            var customer = new Customer(userId, "customer-test", "customer-email-test");
            var inventoryItem = new InventoryItem(customer);

            var catalogItemId = Guid.NewGuid().ToString();
            var item = new InventoryItemLine(catalogItemId, 1);

            inventoryItem.AddNewItemLine(item);

            item = new InventoryItemLine(catalogItemId, 1);
            inventoryItem.AddNewItemLine(item);

            inventoryItem.Customer.CustomerId.Should().Be(userId);
            inventoryItem.Items.Count.Should().Be(1);
            inventoryItem.Items.First(x => x.CatalogItemId.Equals(catalogItemId)).Quantity.Should().Be(2);
        }

        [Fact]
        public void Should_Create_Valid_Empty_InventoryItem_3()
        {
            var userId = Guid.NewGuid().ToString();
            var customer = new Customer(userId, "customer-test", "customer-email-test");
            var inventoryItem = new InventoryItem(customer);

            var catalogItemId1 = Guid.NewGuid().ToString();
            inventoryItem.AddNewItemLine(new InventoryItemLine(catalogItemId1, 1));
            inventoryItem.AddNewItemLine(new InventoryItemLine(catalogItemId1, 1));

            var catalogItemId2 = Guid.NewGuid().ToString();
            inventoryItem.AddNewItemLine(new InventoryItemLine(catalogItemId2, 2));

            inventoryItem.Customer.CustomerId.Should().Be(userId);
            inventoryItem.Items.Count.Should().Be(2);
            inventoryItem.Items.First(x => x.CatalogItemId.Equals(catalogItemId1)).Quantity.Should().Be(2);
            inventoryItem.Items.First(x => x.CatalogItemId.Equals(catalogItemId2)).Quantity.Should().Be(2);
        }
    }
}