namespace Play.Inventory.Core.Application.IoC
{
    using Domain.AggregateModel.CatalogItemAggregate;
    using Domain.AggregateModel.CustomerAggregate;
    using Domain.AggregateModel.InventoryItemAggregate;
    using Infra.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    internal static class RepositoriesContainer
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<ICatalogItemRepository, CatalogItemRepository>();
            services.AddSingleton<IInventoryItemRepository, InventoryItemRepository>();

            return services;
        }
    }
}