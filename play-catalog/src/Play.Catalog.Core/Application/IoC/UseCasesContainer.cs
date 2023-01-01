namespace Play.Catalog.Core.Application.IoC
{
    using Common.Application.UseCase;
    using Microsoft.Extensions.DependencyInjection;
    using UseCases.CreateNewCatalogItem;
    using UseCases.GetCatalogItemById;
    using UseCases.UpdateUnitPriceCatalogItem;

    public static class UseCasesContainer
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddTransient<IUseCaseExecutor<GetCatalogItemByIdRequest>, GetCatalogItemByIdUseCase>();
            services.AddTransient<IUseCaseExecutor<CreateNewCatalogItemRequest>, CreateNewCatalogItemUseCase>();
            services
                .AddTransient<IUseCaseExecutor<UpdateUnitPriceCatalogItemRequest>, UpdateUnitPriceCatalogItemUseCase>();

            return services;
        }
    }
}