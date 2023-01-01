namespace Play.Common.Application.Infra.Repositories.Dapr
{
    internal readonly struct StateItemQueryParameter<TEntry>
    {
        public StateItemQueryParameter(string originalKey)
        {
            OriginalKey = originalKey;
            FormattedKey = KeyFormatterHelper.ConstructStateStoreKey(typeof(TEntry).Name, OriginalKey);
        }

        public readonly string OriginalKey;
        public readonly string FormattedKey;
    }
}