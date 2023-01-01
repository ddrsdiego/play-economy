namespace Play.Customer.Core.Domain.AggregateModel.CustomerAggregate
{
    public readonly struct Email
    {
        public Email(string value)
        {
            Value = value;
        }
        
        public string Value { get; }
    }
}