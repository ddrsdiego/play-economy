namespace Play.Inventory.Core.Application.Infra.Repositories
{
    using Dapr.Client;
    using Microsoft.Extensions.Logging;

    public abstract class Repository
    {
        protected Repository(ILogger logger, DaprClient daprClient, AppSettings appSettings)
        {
            Logger = logger;
            DaprClient = daprClient;
            AppSettings = appSettings;
        }

        protected ILogger Logger { get; }
        protected DaprClient DaprClient { get; }
        protected AppSettings AppSettings { get; }
    }
}