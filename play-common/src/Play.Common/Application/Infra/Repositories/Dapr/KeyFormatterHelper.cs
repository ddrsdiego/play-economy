namespace Play.Common.Application.Infra.Repositories.Dapr
{
    public static class KeyFormatterHelper
    {
        public static string ConstructStateStoreKey(string entityName, string? key) =>
            $"{entityName.ToLowerInvariant()}-{key}";

        public static string DeconstructStateStoreKey(string entityName, string key)
        {
            var index = key.IndexOf('-');
            return key.Substring(index, key.Length);
        }
    }
}