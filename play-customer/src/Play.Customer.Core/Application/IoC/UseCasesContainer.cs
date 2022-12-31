namespace Play.Customer.Core.Application.IoC
{
    using Microsoft.Extensions.DependencyInjection;
    using UseCases;
    using UseCases.GetCustomerByEmail;
    using UseCases.GetCustomerById;
    using UseCases.RegisterNewCustomer;
    using UseCases.UpdateCustomer;

    public static class UseCasesContainer
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services
                .AddTransient<IUseCaseExecutor<RegisterNewCustomerRequest, RegisterNewCustomerResponse>,
                    RegisterNewCustomerUseCase>();
            services
                .AddTransient<IUseCaseExecutor<UpdateCustomerRequest, UpdateCustomerResponse>, UpdateCustomerUseCase>();
            services
                .AddTransient<IUseCaseExecutor<GetCustomerByEmailRequest, GetCustomerByEmailResponse>,
                    GetCustomerByEmailUseCase>();
            services
                .AddTransient<IUseCaseExecutor<GetCustomerByIdRequest, GetCustomerByIdResponse>,
                    GetCustomerByIdUseCase>();

            return services;
        }
    }
}