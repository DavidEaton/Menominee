using Menominee.Client.MessageHandlers;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menominee.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Services.AddTransient<MenonineeApiAuthorizationMessageHandler>();

            var baseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBaseUrl"));

            builder.Services.AddHttpClient<IPersonDataService, PersonDataService>(
                client => client.BaseAddress = baseAddress)
                .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<ICustomerDataService, CustomerDataService>(
                client => client.BaseAddress = baseAddress)
                .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("OidcConfiguration", options.ProviderOptions);
            });

            await builder.Build().RunAsync();
        }

        private static void ConfigureLogging(
          WebAssemblyHostBuilder builder,
          string section = "Logging")
        {
            builder.Logging.AddConfiguration(builder.Configuration.GetSection(section));
        }
    }
}
