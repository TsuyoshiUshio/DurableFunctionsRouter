using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Router.Startup))]

namespace Router
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient("DurableRouterHttpClient");
            builder.Services.AddSingleton<IAzure>(GetClient());
        }

        private IAzure GetClient()
        {
            var credential = new AzureCredentialsFactory()
                .FromServicePrincipal(
                    Environment.GetEnvironmentVariable("ClientId"),
                    Environment.GetEnvironmentVariable("ClientSecret"),
                    Environment.GetEnvironmentVariable("TenantId"),
                    AzureEnvironment.AzureGlobalCloud);

            return Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credential)
                .WithSubscription(Environment.GetEnvironmentVariable("SubscriptionId"));
        }
    }
}
