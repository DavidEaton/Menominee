using Azure.Identity;
using Menominee.Shared.Models.Tenants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;

namespace Menominee.Api
{
    public static class MSGraphConfiguration
    {
        //Called in Startup - services.ConfigureGraphComponent(configuration)
        public static IServiceCollection ConfigureMSGraphComponent(this IServiceCollection services, IConfiguration configuration)
        {
            //Look at appsettings.Development.json | https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1

            var graphConfig = configuration.GetSection("MSGraphConfig").Get<GraphConfiguration>();

            // Initialize the client credential auth provider
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var clientSecretCredential = new ClientSecretCredential(graphConfig.TenantId, graphConfig.AppId, graphConfig.ClientSecret);

            //you can use a single client instance for the lifetime of the application
            services.AddSingleton(provider => new GraphServiceClient(clientSecretCredential, scopes));

            return services;
        }
    }
}