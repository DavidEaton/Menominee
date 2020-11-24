using Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddTransient<ApiAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IPersonDataService, PersonDataService>(
                client => client.BaseAddress = new Uri(
                    builder.Configuration.GetValue<string>("ApiBaseUrl")))
                .AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<ITenantDataService, TenantDataService>(
                client => client.BaseAddress = new Uri(
                    builder.Configuration.GetValue<string>("ApiBaseUrl")))
                .AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

            await builder.Build().RunAsync();
        }
    }
}
