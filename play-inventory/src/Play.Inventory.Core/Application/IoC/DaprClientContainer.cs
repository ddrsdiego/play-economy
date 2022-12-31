namespace Play.Inventory.Core.Application.IoC
{
    using Microsoft.Extensions.DependencyInjection;

    public static class DaprClientContainer
    {
        public static IServiceCollection AddDapr(this IServiceCollection services)
        {
            var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT");
            var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT");

            services.AddDaprClient(configure =>
            {
                configure.UseHttpEndpoint($"http://localhost:{daprHttpPort}");
                configure.UseGrpcEndpoint($"http://localhost:{daprGrpcPort}");
            });
            
            return services;
        }
    }
}