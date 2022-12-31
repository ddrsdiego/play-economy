namespace Play.Catalog.Core.Application.IoC
{
    using Dapr.Client;
    using Domain.AggregatesModel.CatalogItemAggregate;
    using Domain.SeedWorks;
    using Microsoft.Extensions.DependencyInjection;
    using Repositories;

    public static class DaprStateEntryRepositoryEx
    {
        public static IServiceCollection AddDaprStateEntryRepository<T>(this IServiceCollection services,
            string stateStoreName)
        {
            services.AddSingleton<IRepository<CatalogItem>>(sp =>
            {
                var daprClient = sp.GetRequiredService<DaprClient>();
                return new DaprStateEntryRepository<CatalogItem>("play-catalog-state-store", daprClient);
            });
            return services;
        }
    }
}