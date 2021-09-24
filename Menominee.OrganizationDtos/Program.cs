using Menominee.OrganizationDtos.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Blazored.Toast;

namespace Menominee.OrganizationDtos
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Task.Delay(3000); // This allows time for the debugger to attach before any client code runs

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Services.AddBlazoredToast();

            var baseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBaseUrl"));

            builder.Services.AddHttpClient<IOrganizationDataService, OrganizationDataService>(
                    client => client.BaseAddress = baseAddress);

            await builder.Build().RunAsync();

        }
    }
}
