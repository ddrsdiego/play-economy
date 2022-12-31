namespace Play.Catalog.Core.Application.Repositories
{
    internal static class KeyFormatterHelper
    {
        public static string CreateStateStoreKey(string entityName, string? key)
        {
            return $"{entityName.ToLowerInvariant()}-{key}";
        }
    }
}