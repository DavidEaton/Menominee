using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClientApp.Services;

namespace ClientApp
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

            builder.Services.AddAuthorizationCore();
            
            await builder.Build().RunAsync();
        }
    }
}
