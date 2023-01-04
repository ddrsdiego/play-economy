namespace Play.Customer.Core.Application.IoC
{
    using Microsoft.Extensions.DependencyInjection;
    using Play.Common.Application.UseCase;
    using UseCases.GetCustomerByEmail;
    using UseCases.GetCustomerById;
    using UseCases.RegisterNewCustomer;
    using UseCases.UpdateCustomer;

    public static class UseCasesContainer
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUseCaseExecutor<RegisterNewCustomerRequest>, RegisterNewCustomerUseCase>();
            services.AddScoped<IUseCaseExecutor<GetCustomerByEmailRequest>, GetCustomerByEmailUseCase>();
            services.AddScoped<IUseCaseExecutor<UpdateCustomerRequest>, UpdateCustomerUseCase>();
            services.AddScoped<IUseCaseExecutor<GetCustomerByIdRequest>, GetCustomerByIdUseCase>();

            return services;
        }
    }
}