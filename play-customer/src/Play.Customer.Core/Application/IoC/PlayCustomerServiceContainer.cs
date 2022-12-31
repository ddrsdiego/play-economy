namespace Play.Customer.Core.Application.IoC
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class PlayCustomerServiceContainer
    {
        public static IServiceCollection AddPlayCustomerServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions(configuration);
            services.AddSwagger();
            services.AddDaprClient(configure =>
            {
                configure.UseHttpEndpoint("http://localhost:3300");
                configure.UseGrpcEndpoint("http://localhost:53300");
            });

            services.AddRepositories();
            services.AddUseCases();
            
            return services;
        }
    }
}