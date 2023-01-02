namespace Play.Common.UnitTests.Application.Infra.Repositories.Dapr
{
    using System;
    using Common.Application.Infra.Repositories.Dapr;
    using FluentAssertions;
    using Xunit;

    public sealed class StateEntryNameManagerTest
    {
        [Fact]
        public void Test()
        {
            var stateEntry = new CustomerStateEntry
            {
                UserId = Guid.NewGuid().ToString().Split('-')[0],
                Name = "CUSTOMER_NAME"
            };

            var find = StateEntryNameManager.TryExtractName<CustomerStateEntry>(out var stateEntryName);
            find.Should().BeTrue();
            stateEntryName.Should().Be("customer");
        }
    }
}