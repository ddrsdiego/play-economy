namespace Play.Customer.UnitTest.Domain.AggregateModel.CustomerAggregate
{
    using Core.Domain.AggregateModel.CustomerAggregate;
    using Xunit;

    public class EmailTest
    {
        [Fact]
        public void Test()
        {
            var email = new Email("ddrsdiego@hotmail.com");
        }
    }
}