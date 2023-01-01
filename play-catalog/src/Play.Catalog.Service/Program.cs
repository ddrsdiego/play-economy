namespace Play.Catalog.Service
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Core.Application.Infra.Repositories;
    using Core.Application.IoC;
    using Core.Domain.AggregatesModel.CatalogItemAggregate;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    public abstract class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureServices((context, services) =>
                {
                    services.AddUseCases();
                    
                    services.AddSingleton<ICatalogItemRepository, CatalogItemRepository>();
                    services.AddDaprStateEntryRepositories();
                    
                    services.AddDaprClient(configure =>
                    {
                        configure.UseHttpEndpoint("http://localhost:3100");
                        configure.UseGrpcEndpoint("http://localhost:53100");
                    });
                    services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
                    services.AddApiVersioning(options =>
                    {
                        options.DefaultApiVersion = new ApiVersion(1, 0);
                        options.AssumeDefaultVersionWhenUnspecified = true;
                    });
                    services.AddSwaggerGen(c =>
                        c.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Title = Assembly.GetEntryAssembly()?.GetName().Name
                        }));
                });
    }
}