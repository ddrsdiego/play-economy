namespace Play.Common.UnitTests.Application.Infra.Repositories.Dapr
{
    using System;
    using System.Linq;
    using Common.Application.Infra.Repositories.Dapr;
    using FluentAssertions;
    using Xunit;

    public class StateEntryNameAttributeTest
    {
        [Fact]
        public void Should_Try_Extract_StateEntryName_With_Success()
        {
            var stateEntry = new CustomerStateEntry
            {
                UserId = Guid.NewGuid().ToString().Split('-')[0],
                Name = "CUSTOMER_NAME"
            };

            var att = stateEntry.GetType().CustomAttributes
                .SingleOrDefault(x =>
                    x.AttributeType.FullName != null && x.AttributeType.FullName.Equals(
                        StateEntryNameAttribute.FullNameStateEntryNameAttribute,
                        StringComparison.InvariantCultureIgnoreCase));

            var t = att?.ConstructorArguments[StateEntryNameAttribute.StateEntryNamePosition].Value?.ToString();
            t.Should().Be("customer");
        }
    }

    [StateEntryName("customer")]
    public class CustomerStateEntry
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
    }
}