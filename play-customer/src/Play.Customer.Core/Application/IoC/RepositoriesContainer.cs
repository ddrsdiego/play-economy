namespace Play.Customer.Core.Application.IoC
{
    using Domain.AggregateModel.CustomerAggregate;
    using Infra.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    internal static class RepositoriesContainer
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            return services;
        }
    }
}