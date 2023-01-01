namespace Play.Catalog.Core.Application.Infra.Repositories
{
    using Common.Application.Infra.Repositories.Dapr;
    using Dapr.Client;

    public sealed class CatalogItemDaprRepository : DaprStateEntryRepository<CatalogItemData>
    {
        public CatalogItemDaprRepository(DaprClient daprClient)
            : base(CatalogItemRepository.StateStoreName, daprClient)
        {
        }
    }
}