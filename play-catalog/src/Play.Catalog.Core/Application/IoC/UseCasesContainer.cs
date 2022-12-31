namespace Play.Catalog.Core.Application.IoC
{
    using Microsoft.Extensions.DependencyInjection;
    using UseCases.CreateNewCatalogItem;
    using UseCases.UpdateUnitPriceCatalogItem;

    public static class UseCasesContainer
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services
                .AddTransient<IUseCaseExecutor<CreateNewCatalogItemRequest, CreateNewCatalogItemResponse>,
                    CreateNewCatalogItemUseCase>();
            
            services
                .AddTransient<IUseCaseExecutor<UpdateUnitPriceCatalogItemRequest, UpdateUnitPriceCatalogItemResponse>,
                    UpdateUnitPriceCatalogItemUseCase>();
            
            return services;
        }
    }
}